#!/bin/sh
set -e

# ако вече има env variables (Kubernetes), НЕ прави нищо
if [ -n "$ConnectionStrings__DefaultConnection" ]; then
  echo "Using environment variables (Kubernetes mode)"
else
  echo "Loading Docker secrets from /run/secrets"

  SECRETS_DIR="/run/secrets"

  if [ -d "$SECRETS_DIR" ]; then
    for secret in "$SECRETS_DIR"/*; do
      [ -f "$secret" ] || continue

      name="$(basename "$secret")"

      case "$name" in
        *[!a-zA-Z0-9_]*)
          echo "Skipping invalid env var name: $name"
          continue
          ;;
      esac

      value="$(cat "$secret" | tr -d '\r')"
      export "$name=$value"
    done
  fi
fi

exec "$@"