# Credentials Setup

`Here.Sdk.Common` does **not** require HERE credentials. It is a pure value-type and utility layer.

Credentials are required starting from `Here.Sdk.Core`. This document covers the NuGet publishing credentials for contributors.

## NuGet API Key (contributors only)

### Generate a scoped API key

1. Sign in to [nuget.org](https://www.nuget.org/account/apikeys).
2. Click **Create**.
3. Set **Key Name** to `Here.Sdk.Common`.
4. Under **Select Scopes**, choose **Push new packages and package versions**.
5. Under **Select Packages**, choose **Select Packages** and enter `Here.Sdk.Common`.
6. Set **Expiration** to 365 days.
7. Click **Create**. Copy the key immediately — it is shown only once.

### Register the key in GitHub

```bash
gh secret set NUGET_API_KEY -b "<your-key>" -R rinzler78/Here.Sdk.Common
```

Verify:

```bash
gh secret list -R rinzler78/Here.Sdk.Common
# NUGET_API_KEY    Updated YYYY-MM-DD
```

### Rotation reminder

Create a GitHub issue at day 330 to rotate the key before expiry:

```bash
gh issue create \
  --title "Rotate NUGET_API_KEY (expires in 35 days)" \
  --body "Generate a new scoped NuGet API key and update the NUGET_API_KEY secret." \
  --label "maintenance" \
  -R rinzler78/Here.Sdk.Common
```
