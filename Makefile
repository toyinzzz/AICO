# AICO Project Makefile
# This Makefile provides commands for managing Docker containers and development tasks

# Variables
DEV_COMPOSE = environments/dev/docker-compose.dev.yml
PROD_COMPOSE = environments/prod/docker-compose.prod.yml
QA_COMPOSE = environments/QA/docker-compose.yml

# Default target
.PHONY: help
help:
	@echo "AICO Project Makefile"
	@echo "---------------------"
	@echo "Available commands:"
	@echo "  make dev-build      - Build development Docker images"
	@echo "  make dev-up         - Start development containers"
	@echo "  make dev-down       - Stop development containers"
	@echo "  make dev-restart    - Restart development containers"
	@echo "  make dev-logs       - View development container logs"
	@echo "  make prod-build     - Build production Docker images"
	@echo "  make prod-up        - Start production containers"
	@echo "  make prod-down      - Stop production containers"
	@echo "  make qa-build       - Build QA Docker images"
	@echo "  make qa-up          - Start QA containers"
	@echo "  make qa-down        - Stop QA containers"
	@echo "  make clean          - Remove all containers and images"
	@echo "  make help           - Show this help message"

# Development environment
.PHONY: dev-build
dev-build:
	docker-compose -f $(DEV_COMPOSE) build

.PHONY: dev-up
dev-up:
	docker-compose -f $(DEV_COMPOSE) up -d

.PHONY: dev-down
dev-down:
	docker-compose -f $(DEV_COMPOSE) down

.PHONY: dev-restart
dev-restart:
	docker-compose -f $(DEV_COMPOSE) restart

.PHONY: dev-logs
dev-logs:
	docker-compose -f $(DEV_COMPOSE) logs -f

# Production environment
.PHONY: prod-build
prod-build:
	docker-compose -f $(PROD_COMPOSE) build

.PHONY: prod-up
prod-up:
	docker-compose -f $(PROD_COMPOSE) up -d

.PHONY: prod-down
prod-down:
	docker-compose -f $(PROD_COMPOSE) down

# QA environment
.PHONY: qa-build
qa-build:
	docker-compose -f $(QA_COMPOSE) build

.PHONY: qa-up
qa-up:
	docker-compose -f $(QA_COMPOSE) up -d

.PHONY: qa-down
qa-down:
	docker-compose -f $(QA_COMPOSE) down

# Clean up
.PHONY: clean
clean:
	@echo "Stopping all containers..."
	-docker-compose -f $(DEV_COMPOSE) down
	-docker-compose -f $(PROD_COMPOSE) down
	-docker-compose -f $(QA_COMPOSE) down
	@echo "Removing all AICO images..."
	-docker rmi $$(docker images | grep aico | awk '{print $$1}')
	@echo "Clean up completed." 