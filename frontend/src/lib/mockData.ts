
export const mockRecommendations = [
  {
    id: "1",
    title: "Add trust badges near checkout",
    description: "Display SSL certificates, payment security badges, and customer testimonials near the checkout area to increase trust and reduce abandonment.",
    impact: "High",
    expectedUplift: "+15%",
    difficulty: "Easy",
    estimatedTime: "2 hours",
    category: "Trust & Security",
    implemented: false,
    siteId: "1"
  },
  {
    id: "2",
    title: "Optimize mobile checkout flow", 
    description: "Simplify the mobile checkout process by reducing steps, improving form design, and adding mobile-specific payment options.",
    impact: "High",
    expectedUplift: "+22%",
    difficulty: "Medium", 
    estimatedTime: "1 week",
    category: "Mobile UX",
    implemented: false,
    siteId: "1"
  },
  {
    id: "3",
    title: "A/B test CTA button colors",
    description: "Test different CTA button colors, sizes, and text to find the highest converting combination.",
    impact: "Medium",
    expectedUplift: "+8%",
    difficulty: "Easy",
    estimatedTime: "3 hours", 
    category: "CTA Optimization",
    implemented: true,
    siteId: "2"
  }
];

export const mockSites = [
  {
    id: "1",
    name: "E-commerce Store",
    url: "shop.example.com",
    score: 72,
    conversionRate: 2.4,
    revenue: 125000,
    change: 8.2,
    status: "improving",
    lastAnalyzed: "2 hours ago"
  },
  {
    id: "2",
    name: "SaaS Landing Page", 
    url: "app.example.com",
    score: 45,
    conversionRate: 1.1,
    revenue: 85000,
    change: -3.1,
    status: "declining",
    lastAnalyzed: "1 day ago"
  }
];

export const mockMetrics = [
  { name: 'Jan', conversionRate: 1.2, revenue: 85000, bounceRate: 68 },
  { name: 'Feb', conversionRate: 1.4, revenue: 92000, bounceRate: 65 },
  { name: 'Mar', conversionRate: 1.6, revenue: 105000, bounceRate: 62 },
  { name: 'Apr', conversionRate: 1.8, revenue: 118000, bounceRate: 58 },
  { name: 'May', conversionRate: 1.9, revenue: 125000, bounceRate: 55 },
  { name: 'Jun', conversionRate: 2.1, revenue: 145000, bounceRate: 52 },
];
