
import { useState } from "react";
import { Card, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { BarChart3, TrendingUp, Target } from "lucide-react";

export default function FeaturesSection() {
  const [activeFeature, setActiveFeature] = useState(0);

  const features = [
    {
      icon: <BarChart3 className="w-8 h-8" />,
      title: "Real-time Analytics",
      description: "Monitor your website performance with live data and insights."
    },
    {
      icon: <TrendingUp className="w-8 h-8" />,
      title: "Revenue Optimization",
      description: "AI-powered recommendations to boost your conversion rates."
    },
    {
      icon: <Target className="w-8 h-8" />,
      title: "Smart Targeting",
      description: "Identify and focus on your highest-value visitors."
    }
  ];

  return (
    <section className="container mx-auto px-4 py-20">
      <div className="text-center mb-16">
        <h2 className="text-4xl font-bold mb-4">Powerful Features</h2>
        <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
          Everything you need to optimize your website's performance and maximize revenue.
        </p>
      </div>
      
      <div className="grid md:grid-cols-3 gap-8">
        {features.map((feature, index) => (
          <Card 
            key={index}
            className={`cursor-pointer transition-all duration-300 ${
              activeFeature === index ? 'ring-2 ring-primary' : ''
            }`}
            onMouseEnter={() => setActiveFeature(index)}
          >
            <CardHeader>
              <div className="w-16 h-16 bg-gradient-to-r from-blue-500 to-purple-600 rounded-2xl flex items-center justify-center text-white mb-4">
                {feature.icon}
              </div>
              <CardTitle>{feature.title}</CardTitle>
              <CardDescription>{feature.description}</CardDescription>
            </CardHeader>
          </Card>
        ))}
      </div>
    </section>
  );
}
