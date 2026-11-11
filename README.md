# Product MCP Server

This solution exposes product information through the [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) so that Claude Desktop can discover and call product tools using natural-language questions.

The solution contains two .NET 10 applications:

- **MyProduct.Api** – the product REST API and in-memory product data source.
- **McpTest** – the MCP server. It calls `MyProduct.Api` and exposes the results as MCP tools.

## Architecture

```text
┌─────────────────┐       MCP over HTTP        ┌─────────────────┐
│  Claude Desktop │ ─────────────────────────> │  McpTest        │
│  (AI client)    │   http://localhost:5245/mcp│  MCP server     │
└─────────────────┘                            └────────┬────────┘
														│ HTTP GET
														│ https://localhost:7183/api/product/...
														▼
												┌─────────────────┐
												│ MyProduct.Api    │
												│ REST API         │
												└────────┬────────┘
														│
														▼
												Product repository
```

## Exposed MCP tools

| Tool | Purpose | Example input |
|---|---|---|
| `GetAllProducts` | Returns the complete product list. | No arguments |
| `GetProductById` | Returns one product by its numeric ID. | `id: 1` |
| `GetProductsByCategory` | Returns products in a category. | `categoryname: "Electronics"` |
| `SearchProducts` | Searches products by name. | `search: "phone"` |

Claude chooses one of these tools when a question requires product data. The tool descriptions in `Tools/ProductTool.cs` help Claude understand when each tool is appropriate.

## Prerequisites

Install the following on the development machine:

1. [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
2. [Node.js](https://nodejs.org/) with `npx` available on `PATH`.
3. Claude Desktop.
4. The repository cloned locally.

Verify the command-line prerequisites:

```powershell
dotnet --version
node --version
npx --version
```

## Start the applications

Both applications must be running before Claude Desktop can use the MCP server.

### 1. Start the product API

From the solution directory, open a terminal and run:

```powershell
dotnet run --project ..\MyProduct.Api\MyProduct.Api.csproj --launch-profile https
```

The API HTTPS base URL is `https://localhost:7183`, and its product routes are under `/api/product`.

You can verify it directly:

```powershell
Invoke-RestMethod https://localhost:7183/api/product
```

Swagger is available in Development at:

```text
https://localhost:7183/swagger
```

### 2. Start the MCP server

Open a second terminal in this project directory and run:

```powershell
dotnet run --project .\McpTest.csproj --launch-profile http
```

The MCP endpoint used by Claude Desktop is:

```text
http://localhost:5245/mcp
```

Keep this terminal open. The MCP server must remain running while Claude Desktop sends requests.

> The MCP server uses HTTP on port `5245` for the local Claude Desktop connection. It calls the product API over HTTPS on port `7183`. If the API uses the local ASP.NET Core development certificate, trust it with `dotnet dev-certs https --trust` when prompted or when requests fail because of certificate validation.

## Configure Claude Desktop

Claude Desktop reads MCP server definitions from its configuration file. On Windows, the usual file is:

```text
%APPDATA%\Claude\claude_desktop_config.json
```

Create the file if it does not exist. Add the following entry inside the top-level `mcpServers` object:

```json
{
  "mcpServers": {
	"my-product-mcp": {
	  "command": "cmd",
	  "args": ["/c", "npx", "-y", "mcp-remote", "http://localhost:5245/mcp"]
	}
  }
}
```

If the file already contains other MCP servers, preserve them and add only the `my-product-mcp` property. The file must remain valid JSON; do not add a trailing comma after the final property.

After saving the file:

1. Completely quit Claude Desktop, including its tray process.
2. Start Claude Desktop again.
3. Open the MCP/tools area and confirm that `my-product-mcp` is connected.
4. Keep both .NET applications running while testing.

The `mcp-remote` package acts as the local bridge between Claude Desktop's process-based MCP configuration and this HTTP MCP endpoint. `npx -y` downloads or uses the package without requiring a global installation.

## Sample questions for Claude

Try questions such as:

- `List all available products with their names, categories, prices, and stock quantities.`
- `What product has ID 1?`
- `Show me all products in the Electronics category.`
- `Find products whose name contains "phone".`
- `Which products are in the Furniture category?`
- `Search for products related to "laptop" and summarize their prices.`
- `Can you compare the prices of the products returned by the phone search?`

For questions that require product data, Claude should call the appropriate MCP tool instead of inventing an answer. You can ask Claude to explain which tool it used if you want to inspect the interaction.

## Request and response flow

1. **User asks Claude a question.** For example: “Show products in Electronics.”
2. **Claude interprets the request** and matches it to the tool description for `GetProductsByCategory`.
3. **Claude Desktop starts the configured command** (`cmd /c npx -y mcp-remote ...`) or uses the existing bridge process.
4. **`mcp-remote` connects to** `http://localhost:5245/mcp` and forwards MCP messages.
5. **McpTest receives the tool request** through its HTTP transport at `/mcp`.
6. **The MCP runtime invokes `ProductTool.GetProductsByCategory`** with the category supplied by Claude.
7. **`ProductService` calls the REST API** at `https://localhost:7183/api/product/category/{categoryname}` using the named `HttpClient` configured in `Program.cs`.
8. **MyProduct.Api retrieves the products** from its repository and returns JSON.
9. **McpTest deserializes the JSON** into `Product` objects and returns the tool result to Claude Desktop.
10. **Claude uses the result** to formulate the final natural-language response.

```text
Question
   │
   ▼
Claude Desktop
   │ selects MCP tool and arguments
   ▼
mcp-remote
   │ forwards MCP request
   ▼
McpTest /mcp
   │ invokes ProductTool
   ▼
ProductService
   │ GET request
   ▼
MyProduct.Api /api/product/...
   │ JSON response
   └───────────────► back through the same path to Claude
						 │
						 ▼
				   Final answer
```

## Troubleshooting

### Claude does not show the server

- Confirm the JSON file is at `%APPDATA%\Claude\claude_desktop_config.json`.
- Confirm the server key is inside `mcpServers`.
- Fully restart Claude Desktop after changing the file.
- Confirm `npx --version` works from PowerShell and from `cmd`.
- Check that the MCP server is running on `http://localhost:5245/mcp`.

### The MCP server cannot connect

- Start `McpTest` with the `http` launch profile.
- Check that port `5245` is not already in use.
- Open `http://localhost:5245/` to confirm the ASP.NET Core process is running. The root response should be `Hello World!`.
- Review the MCP server terminal for startup or transport errors.

### Product tool calls fail

- Start `MyProduct.Api` with the `https` launch profile.
- Confirm `https://localhost:7183/api/product` returns JSON.
- Trust the local development certificate:

  ```powershell
  dotnet dev-certs https --trust
  ```

- If the API port differs, update the `BaseAddress` in `Program.cs` in the MCP project.
- Start the API before calling a product tool.

### A search returns no products

Search and category values are sent as URL path segments. Use a product name or category that exists in the API data, and avoid relying on punctuation or URL characters that may need encoding.

## Development notes

- The MCP endpoint is mapped in `Program.cs` with `app.MapGroup("/mcp").MapMcp()`.
- MCP tools are discovered from the assembly with `.WithToolsFromAssembly()`.
- Product tools are declared in `Tools/ProductTool.cs` using `[McpServerTool]` and `[Description]` attributes.
- The current local setup has no authentication and is intended for development use only. Do not expose the HTTP MCP endpoint publicly without adding appropriate authentication, authorization, HTTPS, and network protections.
