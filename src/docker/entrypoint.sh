#!/bin/sh
set -e

SECRETS_DIR="/run/secrets"

if [ -d "$SECRETS_DIR" ]; then
  for secret in "$SECRETS_DIR"/*; do
    name="$(basename "$secret")"
    value="$(cat "$secret" | tr -d '\r')"
    export "$name=$value"
  done
fi

exec "$@"