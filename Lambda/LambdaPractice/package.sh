#!/usr/bin/env zsh
set -euo pipefail

# Package project into LambdaPractice.zip excluding build and tooling artifacts.

# Move to repo root (directory of this script)
SCRIPT_DIR=${0:a:h}
cd "$SCRIPT_DIR"

ZIP_NAME="LambdaPractice.zip"

# Ensure zip is available
if ! command -v zip >/dev/null 2>&1; then
  echo "Error: 'zip' command not found. Install it (e.g., 'brew install zip')." >&2
  exit 1
fi

# Remove previous archive if present
if [[ -f "$ZIP_NAME" ]]; then
  rm -f "$ZIP_NAME"
fi

# Create archive from current directory, excluding build artifacts and editor/OS cruft
zip -r "$ZIP_NAME" . \
  -x "./$ZIP_NAME" \
  -x "*.DS_Store" -x "*/.DS_Store" \
  -x "bin/*" -x "obj/*" -x "*/bin/*" -x "*/obj/*" \
  -x ".git/*" -x ".vs/*" -x ".vscode/*" \
  -x "**/.git/*" -x "**/.vs/*" -x "**/.vscode/*" \
  -x "**/node_modules/*" \
  -x "*.user" -x "*.suo"

# Summary
if [[ -f "$ZIP_NAME" ]]; then
  echo "Created $ZIP_NAME"
  if command -v stat >/dev/null 2>&1; then
    if [[ "$(uname)" == "Darwin" ]]; then
      stat -f "%z bytes" "$ZIP_NAME"
    else
      stat -c "%s bytes" "$ZIP_NAME"
    fi
  fi
else
  echo "Failed to create $ZIP_NAME" >&2
  exit 1
fi
