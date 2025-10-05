#!/bin/sh

# Find solution root (parent directory of this script)
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
SOLUTION_ROOT="$(dirname "$SCRIPT_DIR")"
GIT_DIR="$SOLUTION_ROOT/.git"
HOOKS_DIR="$GIT_DIR/hooks"

PRE_COMMIT_FILE="$HOOKS_DIR/pre-commit"
# Prompt before overwriting existing pre-commit hook
if [ -f "$PRE_COMMIT_FILE" ]; then
  read -p "A pre-commit hook already exists. Overwrite it? (y/N): " overwrite
  case "$overwrite" in
    y|Y ) echo "Overwriting pre-commit hook...";;
    * ) echo "Skipping pre-commit hook installation."; exit 0;;
  esac
fi

cat << 'EOF' > "$PRE_COMMIT_FILE"
#!/bin/sh

# Colors
RED="$(tput setaf 1)"
GREEN="$(tput setaf 2)"
YELLOW="$(tput setaf 3)"
RESET="$(tput sgr0)"

# Fail if there are unstaged changes
if ! git diff --quiet; then
  echo "${YELLOW}Warning: You have unstaged changes. Please stage them before committing.${RESET}"
  exit 1
fi

echo "${YELLOW}Running dotnet format...${RESET}"
dotnet format
format_status=$?

echo "${YELLOW}Running dotnet build (with analyzers)...${RESET}"
dotnet build --no-restore --nologo
build_status=$?

if [ $format_status -ne 0 ] || [ $build_status -ne 0 ]; then
  echo "${RED}There were formatting or analyzer warnings/errors.${RESET}"
  # Check if running in interactive shell
  if [ -t 0 ]; then
    read -p "Continue with commit anyway? (y/N): " choice
    case "$choice" in
      y|Y ) echo "${YELLOW}Continuing with commit...${RESET}";;
      * ) echo "${RED}Commit aborted.${RESET}"; exit 1;;
    esac
  else
    echo "${RED}Commit aborted due to Warnings/Errors. To override, commit from a terminal (CLI) and confirm when prompted.${RESET}"
    if [ -f "$GIT_DIR/COMMIT_EDITMSG" ]; then
      echo "To continue, run:"
      echo "  git commit -F $GIT_DIR/COMMIT_EDITMSG"
    fi
    exit 1
  fi
else
  echo "${GREEN}All pre-commit checks passed.${RESET}"
fi
exit 0
EOF

chmod +x "$HOOKS_DIR/pre-commit"


# Restore NuGet packages
dotnet restore

echo "Setup complete. Pre-commit hook installed, dotnet format and analyzers enforced, and packages restored."
