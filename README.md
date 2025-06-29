##  Apply Initial Migration

To create the initial database migration, run:

```bash
add-migration Initial -OutputDir Infrastructure/Persistence/Migrations -Project Catalog
```
This command generates migration files under:

```
Infrastructure/Persistence/Migrations
```

in the `Catalog` project.

---

## Deployment to Azure Container Apps (ACA)

### Prerequisites

- **Azure subscription** (e.g., Free Trial)  
- **`azd` CLI** installed locally  

### Authenticate & Verify CLI

```bash
azd version      # Confirm CLI version
azd auth login   # Sign in to Azure
```

### Provision & Deploy

```bash
azd init   # Initialize the project (select template/env as prompted)
azd up     # Provision Azure resources and deploy the solution
```

### Tear Down Resources

```bash
azd down   # Destroy all Azure resources created by 'azd up'
```

## AI Integration with Ollama
- Used Ollama to run AI models locally
- Integrated llama3:2 to provide support chat capabilities.
- Integrated all-minilm to generate vector embeddings for semantic search features.


---

## Resources

![image](https://github.com/user-attachments/assets/22d407e9-79ed-4ffc-a268-ba9dfb21fb38)

---
