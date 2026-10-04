# =========================
#  Frontend - Multi-stage
# =========================

# 1) Build aşaması (Node)
FROM node:22-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

# 2) Runtime aşaması (nginx)
FROM nginx:alpine AS runtime

# Özel nginx config (API isteklerini backend'e proxy'ler)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Build çıktısını nginx'in serve edeceği dizine kopyala
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
    CMD wget -qO- http://localhost/ >/dev/null || exit 1

CMD ["nginx", "-g", "daemon off;"]