# Security Policy

## Reporting a Vulnerability

Please **do not** open a public GitHub issue for security vulnerabilities.

Report vulnerabilities privately via GitHub's [Security Advisories](https://github.com/rinzler78/Here.Sdk.Common/security/advisories/new) feature.

We aim to acknowledge reports within 48 hours and provide a fix within 14 days for critical issues.

## Scope

This package contains no credentials, no network I/O, and no secret-handling code. Security concerns are limited to:
- Malformed input causing panics or memory issues (e.g. in `FlexiblePolyline.Decode`)
- Incorrect WGS84 validation allowing invalid geographic data to propagate downstream
