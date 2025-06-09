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
- A **.NET Aspire** solution with container-based microservices

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

---

## Resources

![image](https://github.com/user-attachments/assets/bea504e5-9037-456c-aad0-8fdd5135c2e2)

---
