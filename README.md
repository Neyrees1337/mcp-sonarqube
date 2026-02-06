# MCP SonarQube Server

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![MCP](https://img.shields.io/badge/MCP-Compatible-blue)](https://modelcontextprotocol.io/)

A [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) server for SonarQube integration, enabling AI assistants to interact with SonarQube Projects, Issues, Quality Gates, Measures, Security Hotspots, Rules, and System Health.

---

## Quick Start

```bash
# 1. Clone the repository
git clone https://github.com/viamus/mcp-sonarqube.git
cd mcp-sonarqube

# 2. Configure credentials
cp .env.example .env
# Edit .env with your SonarQube URL and token

# 3. Run the server
docker compose up -d
```

---

## Prerequisites

| Requirement | Version | Purpose |
|-------------|---------|---------|
| .NET SDK | 10.0+ | Build and run |
| Docker | Latest | Container deployment (optional) |
| SonarQube Instance | 9.x+ | API access |

**Required:**
- SonarQube base URL (e.g., `https://sonarqube.example.com`)
- SonarQube user token (generate at: Your SonarQube > My Account > Security > Tokens)

---

## Available Tools

### Project Tools

| Tool | Description | SonarQube API |
|------|-------------|---------------|
| `search_projects` | Search for SonarQube projects by name or key | `/api/projects/search` |
| `get_project_status` | Get quality gate status and key measures for a project | `/api/qualitygates/project_status` + `/api/measures/component` |

### Issue Tools

| Tool | Description | SonarQube API |
|------|-------------|---------------|
| `search_issues` | Search for issues (bugs, vulnerabilities, code smells) with filters | `/api/issues/search` |

### Measure Tools

| Tool | Description | SonarQube API |
|------|-------------|---------------|
| `get_measures` | Get metrics for a component (coverage, bugs, vulnerabilities, etc.) | `/api/measures/component` |

### Quality Gate Tools

| Tool | Description | SonarQube API |
|------|-------------|---------------|
| `list_quality_gates` | List all available quality gates with conditions | `/api/qualitygates/list` |

### Hotspot Tools

| Tool | Description | SonarQube API |
|------|-------------|---------------|
| `search_hotspots` | Search for security hotspots in a project | `/api/hotspots/search` |
| `get_hotspot` | Get detailed information about a specific hotspot | `/api/hotspots/show` |

### System Tools

| Tool | Description | SonarQube API |
|------|-------------|---------------|
| `get_health` | Get the health status of the SonarQube instance | `/api/system/health` |

### Rule Tools

| Tool | Description | SonarQube API |
|------|-------------|---------------|
| `search_rules` | Search for coding rules by language, severity, or tags | `/api/rules/search` |

---

## Running Options

### Option 1: Docker Compose (Recommended)

```bash
docker compose up -d
```

The server will be available at `http://localhost:8201`.

### Option 2: .NET CLI

```bash
dotnet run --project src/Viamus.Sonarqube.Mcp.Server
```

The server will be available at `http://localhost:5100`.

### Option 3: Self-Contained Executable

```bash
dotnet publish src/Viamus.Sonarqube.Mcp.Server -c Release -o ./publish
./publish/Viamus.Sonarqube.Mcp.Server
```

---

## Client Configuration

### Claude Desktop

Add to your Claude Desktop configuration (`claude_desktop_config.json`):

```json
{
  "mcpServers": {
    "sonarqube": {
      "url": "http://localhost:8201/sse"
    }
  }
}
```

### Claude Code

```bash
claude mcp add sonarqube --transport sse http://localhost:8201/sse
```

---

## Usage Examples

### Search for projects

```
Search for all projects containing "backend" in my SonarQube instance.
```

### Check project quality

```
What is the quality gate status for the "my-app" project? Show me the coverage and bug count.
```

### Find critical issues

```
Search for all CRITICAL and BLOCKER severity issues in the "my-app" project.
```

### Review security hotspots

```
Show me all security hotspots that need review in the "my-app" project.
```

### Search coding rules

```
Find all MAJOR severity rules for C# language.
```

### Check system health

```
Is my SonarQube instance healthy?
```

---

## Configuration

### Environment Variables

| Variable | Required | Description |
|----------|----------|-------------|
| `SONARQUBE_BASE_URL` | Yes | SonarQube instance URL |
| `SONARQUBE_TOKEN` | Yes | SonarQube user token |
| `SERVER_REQUIRE_API_KEY` | No | Enable API key authentication (default: `false`) |
| `SERVER_API_KEY` | No | API key for server access |

### appsettings.json

```json
{
  "SonarQube": {
    "BaseUrl": "https://your-sonarqube-instance.com",
    "Token": "your-token-here"
  },
  "ServerSecurity": {
    "RequireApiKey": false,
    "ApiKey": ""
  }
}
```

### User Secrets (Development)

```bash
cd src/Viamus.Sonarqube.Mcp.Server
dotnet user-secrets set "SonarQube:BaseUrl" "https://your-sonarqube-instance.com"
dotnet user-secrets set "SonarQube:Token" "your-token-here"
```

---

## Troubleshooting

### Common Issues

**Connection refused**
- Verify the SonarQube base URL is correct and accessible
- Check that the server is running: `curl http://localhost:8201/health`

**401 Unauthorized from SonarQube**
- Verify your token is valid and not expired
- Generate a new token at: Your SonarQube > My Account > Security > Tokens

**No projects found**
- Ensure your token has sufficient permissions
- Verify the project exists in your SonarQube instance

**Docker container not starting**
- Check logs: `docker compose logs mcp-sonarqube`
- Verify `.env` file exists and contains valid credentials

---

## Project Structure

```
mcp-sonarqube/
├── src/Viamus.Sonarqube.Mcp.Server/
│   ├── Configuration/          # Settings classes
│   ├── Middleware/              # API key authentication
│   ├── Models/                  # SonarQube API DTOs
│   ├── Services/                # HTTP client for SonarQube API
│   ├── Tools/                   # MCP tool implementations (10 tools)
│   └── Program.cs               # Entry point
├── tests/Viamus.Sonarqube.Mcp.Server.Tests/
│   ├── Configuration/           # Settings and middleware tests
│   ├── Models/                  # Serialization tests
│   └── Tools/                   # Tool behavior tests
├── docker-compose.yml
└── Solution.slnx
```

---

## API Reference

### SonarQube API Endpoints Used

| Endpoint | Tool |
|----------|------|
| `/api/projects/search` | `search_projects` |
| `/api/qualitygates/project_status` | `get_project_status` |
| `/api/measures/component` | `get_project_status`, `get_measures` |
| `/api/issues/search` | `search_issues` |
| `/api/qualitygates/list` | `list_quality_gates` |
| `/api/hotspots/search` | `search_hotspots` |
| `/api/hotspots/show` | `get_hotspot` |
| `/api/system/health` | `get_health` |
| `/api/rules/search` | `search_rules` |

---

## Development

### Building

```bash
dotnet build Solution.slnx
```

### Testing

```bash
dotnet test Solution.slnx
```

### Adding New Tools

See [CONTRIBUTING.md](CONTRIBUTING.md#adding-a-new-mcp-tool) for detailed instructions.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
