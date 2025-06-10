# AICO Frontend

<div align="center">
  <h3>🚀 AI Conversion Optimizer - Frontend</h3>
  
  [![React](https://img.shields.io/badge/React-18.3.1-blue.svg)](https://reactjs.org/)
  [![TypeScript](https://img.shields.io/badge/TypeScript-5.5.3-blue.svg)](https://www.typescriptlang.org/)
  [![Vite](https://img.shields.io/badge/Vite-5.4.1-646CFF.svg)](https://vitejs.dev/)
  [![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-3.4.11-38B2AC.svg)](https://tailwindcss.com/)
</div>

## Overview

The frontend component of the AICO platform built with React, TypeScript, and Vite. AICO analyzes websites and provides AI-driven recommendations to optimize performance, increase conversions, and enhance user experience.

## Features

- **Website Analysis**: Comprehensive website performance and conversion analysis
- **Dashboard Analytics**: Real-time metrics and visualization
- **AI-Powered Insights**: Machine learning recommendations for optimization
- **Competitor Analysis**: Competitive intelligence and benchmarking
- **CRO Expert Chat**: AI-powered consultation interface
- **User Management**: Authentication, profile, and settings

## Tech Stack

### Core Framework
- **React 18.3.1** - Modern React with concurrent features
- **TypeScript 5.5.3** - Type-safe development
- **Vite 5.4.1** - Fast build tool and dev server

### UI & Styling
- **Tailwind CSS 3.4.11** - Utility-first CSS framework
- **shadcn/ui** - High-quality component library
- **Radix UI** - Accessible component primitives
- **Lucide React** - Icon library
- **next-themes** - Dark/light mode support

### State Management & Data
- **TanStack Query 5.56.2** - Server state management
- **React Hook Form 7.53.0** - Form handling
- **Zod 3.23.8** - Schema validation

### Routing & Navigation
- **React Router DOM 6.26.2** - Client-side routing

### Charts & Visualization
- **Recharts 2.12.7** - Composable charting library
- **jsPDF 3.0.1** - PDF report generation

## Project Structure

```
frontend/
├── src/
│   ├── components/          # Reusable UI components
│   │   ├── Analysis/        # Website analysis components
│   │   ├── CROExpertChat/   # AI chat interface
│   │   ├── Dashboard/       # Dashboard widgets and charts
│   │   ├── Landing/         # Landing page sections
│   │   ├── Layout/          # Layout components
│   │   ├── Pricing/         # Pricing page components
│   │   └── ui/              # shadcn/ui base components
│   ├── hooks/               # Custom React hooks
│   ├── lib/                 # Utility libraries
│   ├── pages/               # Page components
│   ├── providers/           # Context providers
│   └── utils/               # Utility functions
├── public/                  # Static assets
├── environments/            # Environment-specific configurations
│   ├── dev/                 # Development environment
│   ├── prod/                # Production environment
│   └── QA/                  # Quality Assurance environment
└── [config files]           # Various configuration files
```

## Getting Started

### Prerequisites
- **Node.js** 18+ or **Bun** (recommended)
- **npm**, **yarn**, or **bun** package manager
- **Docker** (optional, for containerized development)

### Installation

```bash
# Clone the repository
git clone https://github.com/toyinzzz/AICO.git
cd AICO/frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

### Docker Development

```bash
# Run in Docker development container
.\run-dev.bat
```

## Available Scripts

```bash
# Start development server
npm run dev

# Build for production
npm run build

# Build for development
npm run build:dev

# Preview production build
npm run preview

# Run linting
npm run lint
```

## Environment Configuration

Create a `.env` file for environment-specific configuration:

```
VITE_API_BASE_URL=http://localhost:5000/api
VITE_AI_SERVICE_URL=http://localhost:8000
VITE_ENVIRONMENT=development
```

## Deployment

### Standard Deployment

```bash
# Build for production
npm run build

# Deploy the contents of the dist directory to your hosting provider
```

### Docker Deployment

```bash
# Build and deploy using Docker
.\docker-prod-deploy.bat
```

## Development Guidelines

### Code Style
- Follow TypeScript best practices
- Use functional components with hooks
- Maintain consistent naming conventions
- Write self-documenting code with appropriate comments

### Component Guidelines
- Keep components focused on a single responsibility
- Use composition over inheritance
- Ensure proper accessibility (ARIA) attributes
- Implement responsive design for all screen sizes

### State Management
- Use local state for component-specific data
- Use context for shared state across components
- Use TanStack Query for server state management

## Integration with Backend

The frontend communicates with the C# backend API through RESTful endpoints. See the backend README for more information on API endpoints and integration. 