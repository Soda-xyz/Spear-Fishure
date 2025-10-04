# Project Setup Instructions

After cloning this repository, run the setup script to install the pre-commit hook and restore NuGet packages.

## Steps

1. Open a terminal in the project root directory.
2. Run the following command:

   ```sh
   sh setup.sh
   ```

   If you get a permission error, make the script executable first:

   ```sh
   chmod +x setup.sh
   ./setup.sh
   ```

This will:
- Copy the pre-commit hook to `.git/hooks/pre-commit`
- Make it executable
- Restore all NuGet packages

Now you are ready to develop and commit with automatic linting checks!
