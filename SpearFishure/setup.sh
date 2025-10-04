#!/bin/sh

# Copy pre-commit hook
cp hooks/pre-commit .git/hooks/pre-commit
chmod +x .git/hooks/pre-commit

# Restore NuGet packages
dotnet restore

echo "Setup complete. Pre-commit hook installed and packages restored."
