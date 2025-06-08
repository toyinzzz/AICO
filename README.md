```
# AICO - AI Conversion Optimizer Frontend

<div align="center">
  <h3>🚀 Analyze any website, recommend CRO fixes, and 
  show before-vs-after revenue impact in a live 
  dashboard</h3>
  
  [![React](https://img.shields.io/badge/React-18.3.
  1-blue.svg)](https://reactjs.org/)
  [![TypeScript](https://img.shields.io/badge/
  TypeScript-5.5.3-blue.svg)](https://www.
  typescriptlang.org/)
  [![Vite](https://img.shields.io/badge/Vite-5.4.
  1-646CFF.svg)](https://vitejs.dev/)
  [![Tailwind CSS](https://img.shields.io/badge/
  Tailwind_CSS-3.4.11-38B2AC.svg)](https://tailwindcss.
  com/)
  [![shadcn/ui](https://img.shields.io/badge/
  shadcn%2Fui-Latest-000000.svg)](https://ui.shadcn.
  com/)
</div>

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Development](#development)
- [Architecture](#architecture)
- [Components](#components)
- [Deployment](#deployment)
- [Contributing](#contributing)

## 🎯 Overview

AICO Frontend is a modern React application built with 
TypeScript that provides an intuitive interface for 
website conversion rate optimization. It's part of a 
microservices architecture that includes a C# backend 
API and Python AI analysis service.

### Key Capabilities
- **Website Analysis**: Comprehensive website 
performance and conversion analysis
- **Real-time Dashboard**: Live metrics and analytics 
visualization
- **AI-Powered Insights**: Machine learning 
recommendations for optimization
- **Competitor Analysis**: Competitive intelligence 
and benchmarking
- **CRO Expert Chat**: AI-powered consultation 
interface
- **Report Generation**: Automated PDF report creation

## ✨ Features

### 🏠 Landing & Marketing
- Modern, responsive landing page
- Pricing plans and feature comparison
- Contact forms and support pages
- Terms of service and privacy policy

### 📊 Analytics Dashboard
- Real-time conversion rate tracking
- Revenue impact visualization
- User journey funnel analysis
- Performance trend charts
- Key metrics overview (CTR, bounce rate, etc.)

### 🔍 Website Analysis
- URL input and validation
- Comprehensive site analysis results
- Performance metrics (Core Web Vitals)
- SEO analysis and recommendations
- Conversion optimization suggestions
- Before/after comparison views

### 🏆 Competitor Analysis
- Market intelligence dashboard
- Feature comparison matrices
- Performance benchmarking
- Competitive trend analysis

### 💬 AI Chat Interface
- CRO expert consultation
- Implementation guidance
- Best practice recommendations
- Custom optimization strategies

### ⚙️ User Management
- User authentication (Login/Signup)
- Profile management
- Settings and preferences
- Notification center

## 🛠 Tech Stack

### Core Framework
- **React 18.3.1** - Modern React with concurrent 
features
- **TypeScript 5.5.3** - Type-safe development
- **Vite 5.4.1** - Fast build tool and dev server

### UI & Styling
- **Tailwind CSS 3.4.11** - Utility-first CSS framework
- **shadcn/ui** - High-quality component library
- **Radix UI** - Accessible component primitives
- **Lucide React** - Beautiful icon library
- **next-themes** - Dark/light mode support

### State Management & Data
- **TanStack Query 5.56.2** - Server state management
- **React Hook Form 7.53.0** - Form handling
- **Zod 3.23.8** - Schema validation

### Routing & Navigation
- **React Router DOM 6.26.2** - Client-side routing

### Charts & Visualization
- **Recharts 2.12.7** - Composable charting library
- **jsPDF 3.0.1** - PDF generation

### Development Tools
- **ESLint** - Code linting
- **TypeScript ESLint** - TypeScript-specific linting
- **PostCSS** - CSS processing
- **Autoprefixer** - CSS vendor prefixing

## 📁 Project Structure

```
src/
├── components/              # Reusable UI components
│   ├── Analysis/            # Website analysis components
│   ├── CROExpertChat/       # AI chat interface
│   ├── Dashboard/           # Dashboard widgets and charts
│   ├── Landing/             # Landing page sections
│   ├── Layout/              # Layout components (Header, Sidebar)
│   ├── Pricing/             # Pricing page components
│   └── ui/                  # shadcn/ui base components
├── hooks/                   # Custom React hooks
│   ├── use-mobile.tsx       # Mobile detection hook
│   └── use-toast.ts         # Toast notification hook
├── lib/                     # Utility libraries
│   ├── mockData.ts          # Mock data for development
│   └── utils.ts             # Utility functions
├── pages/                   # Page components
│   ├── About.tsx            # About page
│   ├── Analysis.tsx         # Website analysis page
│   ├── CompetitorAnalysis.tsx # Competitor analysis page
│   ├── Contact.tsx          # Contact page
│   ├── Dashboard.tsx        # Main dashboard
│   ├── Landing.tsx          # Landing page
│   ├── Login.tsx            # Login page
│   ├── Pricing.tsx          # Pricing page
│   ├── Profile.tsx          # User profile
│   ├── Settings.tsx         # User settings
│   └── ...                  # Other pages
├── providers/               # React context providers
│   └── ThemeProvider.tsx    # Theme context provider
├── utils/                   # Utility functions
│   └── reportGenerator.ts   # PDF report generation
└── types/                   # TypeScript type definitions

```

## 🚀 Getting Started

### Prerequisites
- **Node.js** 18+ or **Bun** (recommended)
- **npm**, **yarn**, or **bun** package manager

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd AICO
```
2. Install dependencies
   
   ```
   # Using npm
   npm install
   
   # Using bun (recommended)
   bun install
   ```
3. Start development server
   
   ```
   # Using npm
   npm run dev
   
   # Using bun
   bun dev
   ```
4. Open your browser Navigate to http://localhost:8080
## 💻 Development
### Available Scripts
```
# Start development server
npm run dev

# Build for production
npm run build

# Build for development
npm run build:dev

# Preview production build
npm run preview

# Run linting
npm run lint
```
### Development Features
- Hot Module Replacement (HMR) - Instant updates during development
- TypeScript Support - Full type checking and IntelliSense
- ESLint Integration - Code quality and consistency
- Path Aliases - Clean imports with @/ prefix
- Component Tagging - Development-time component identification
### Environment Configuration
The application uses Vite's environment configuration:

- Development : http://localhost:8080
- Host : :: (accepts connections from any IP)
- Port : 8080
## 🏗 Architecture
### Component Architecture
The application follows a modular component architecture:

- Pages : Top-level route components
- Components : Reusable UI components organized by feature
- Hooks : Custom React hooks for shared logic
- Providers : Context providers for global state
- Utils : Utility functions and helpers
### State Management
- TanStack Query : Server state, caching, and synchronization
- React Context : Global UI state (theme, user preferences)
- Local State : Component-specific state with useState
- Form State : Form handling with React Hook Form
### Styling Architecture
- Tailwind CSS : Utility-first styling
- CSS Variables : Theme customization
- Component Variants : Consistent component styling with CVA
- Responsive Design : Mobile-first approach
## 🧩 Components
### Core Components Layout Components
- ModernSidebar : Navigation sidebar with collapsible menu
- Header : Top navigation bar with user actions
- ThemeProvider : Dark/light mode management Dashboard Components
- MoneyLostChart : Revenue impact visualization
- MetricCards : Key performance indicators
- TrendCharts : Performance trend analysis
- FunnelAnalysis : User journey visualization Analysis Components
- SiteAnalysisForm : URL input and validation
- AnalysisResults : Comprehensive analysis display
- RecommendationCards : Optimization suggestions
- PerformanceMetrics : Core Web Vitals display UI Components (shadcn/ui)
- Button : Customizable button component
- Card : Content container component
- Dialog : Modal dialog component
- Form : Form components with validation
- Chart : Chart container with tooltips
- Toast : Notification system
### Component Guidelines
- TypeScript : All components are fully typed
- Accessibility : ARIA compliance and keyboard navigation
- Responsive : Mobile-first responsive design
- Reusable : Modular and composable components
- Consistent : Unified design system
## 🐳 Deployment
### Docker Deployment
The application includes Docker configuration for containerized deployment:

```
# Multi-stage build for optimized production image
FROM node:18-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```
### Production Build
```
# Create production build
npm run build

# Preview production build locally
npm run preview
```
### Environment Variables
Create a .env file for environment-specific configuration:

```
VITE_API_BASE_URL=http://localhost:5000/api
VITE_AI_SERVICE_URL=http://localhost:8000
VITE_ENVIRONMENT=development
```
## 🤝 Contributing
### Development Workflow
1. Fork the repository
2. Create a feature branch
   ```
   git checkout -b feature/amazing-feature
   ```
3. Make your changes
4. Run tests and linting
   ```
   npm run lint
   npm run build
   ```
5. Commit your changes
   ```
   git commit -m 'Add amazing feature'
   ```
6. Push to the branch
   ```
   git push origin feature/amazing-feature
   ```
7. Open a Pull Request
### Code Standards
- TypeScript : Strict type checking enabled
- ESLint : Follow configured linting rules
- Prettier : Code formatting (if configured)
- Component Structure : Follow established patterns
- Naming Conventions : PascalCase for components, camelCase for functions
### Testing Guidelines
- Write unit tests for utility functions
- Test component behavior, not implementation
- Ensure accessibility compliance
- Test responsive design on multiple devices
## 📚 Additional Resources
- React Documentation
- TypeScript Handbook
- Vite Guide
- Tailwind CSS Documentation
- shadcn/ui Documentation
- TanStack Query Documentation
## 📄 License
This project is part of the AICO platform. See the main repository for license information.

Built with ❤️ by the AICO Team

🚀 Ready to optimize your conversions?