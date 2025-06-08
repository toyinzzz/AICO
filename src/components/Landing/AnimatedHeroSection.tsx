import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { ArrowRight, Zap, TrendingUp, Shield, Clock } from "lucide-react";

export default function AnimatedHeroSection() {
  const [currentFeature, setCurrentFeature] = useState(0);
  
  const features = [
    { icon: TrendingUp, text: "Boost conversions by 40%" },
    { icon: Shield, text: "Enterprise-grade security" },
    { icon: Clock, text: "Real-time analysis" },
    { icon: Zap, text: "AI-powered insights" }
  ];

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentFeature((prev) => (prev + 1) % features.length);
    }, 3000);
    return () => clearInterval(interval);
  }, [features.length]);

  return (
    <section className="relative min-h-screen flex items-center justify-center overflow-hidden bg-gradient-to-br from-background via-background to-muted/20">
      {/* Animated background elements */}
      <div className="absolute inset-0 overflow-hidden">
        <div className="absolute -top-40 -right-40 w-80 h-80 bg-primary/10 rounded-full blur-3xl animate-pulse"></div>
        <div className="absolute -bottom-40 -left-40 w-80 h-80 bg-secondary/10 rounded-full blur-3xl animate-pulse" style={{ animationDelay: '1s' }}></div>
        <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-96 h-96 bg-accent/5 rounded-full blur-3xl animate-pulse" style={{ animationDelay: '2s' }}></div>
      </div>

      <div className="container mx-auto px-4 relative z-10">
        <div className="text-center space-y-8 animate-fade-in">
          <Badge variant="secondary" className="px-4 py-2 text-sm animate-scale-in" style={{ animationDelay: '0.2s' }}>
            <Zap className="w-4 h-4 mr-2" />
            AI-Powered Website Optimization
          </Badge>
          
          <h1 className="text-4xl md:text-6xl lg:text-7xl font-bold bg-gradient-to-r from-foreground via-primary to-foreground bg-clip-text text-transparent leading-tight animate-fade-in" style={{ animationDelay: '0.4s' }}>
            Transform Your Website Into a
            <span className="block bg-gradient-to-r from-primary to-secondary bg-clip-text text-transparent">
              Conversion Machine
            </span>
          </h1>
          
          <p className="text-xl md:text-2xl text-muted-foreground max-w-3xl mx-auto leading-relaxed animate-fade-in" style={{ animationDelay: '0.6s' }}>
            AICO analyzes your website and provides AI-driven recommendations to optimize performance, 
            increase conversions, and enhance user experience.
          </p>

          {/* Rotating features */}
          <div className="flex items-center justify-center space-x-3 h-12 animate-fade-in" style={{ animationDelay: '0.8s' }}>
            {features.map((feature, index) => {
              const Icon = feature.icon;
              return (
                <div
                  key={index}
                  className={`flex items-center space-x-2 px-4 py-2 rounded-full transition-all duration-500 ${
                    index === currentFeature 
                      ? 'bg-primary text-primary-foreground scale-110 shadow-lg' 
                      : 'bg-muted/50 text-muted-foreground scale-95 opacity-60'
                  }`}
                >
                  <Icon className="w-4 h-4" />
                  <span className="text-sm font-medium">{feature.text}</span>
                </div>
              );
            })}
          </div>

          <div className="flex flex-col sm:flex-row items-center justify-center space-y-4 sm:space-y-0 sm:space-x-6 animate-fade-in" style={{ animationDelay: '1s' }}>
            <Link to="/signup">
              <Button size="lg" className="px-8 py-6 text-lg font-semibold group relative overflow-hidden">
                <span className="relative z-10 flex items-center">
                  Start Free Analysis
                  <ArrowRight className="ml-2 w-5 h-5 group-hover:translate-x-1 transition-transform" />
                </span>
                <div className="absolute inset-0 bg-gradient-to-r from-primary to-primary/80 group-hover:from-primary/90 group-hover:to-primary transition-all duration-300"></div>
              </Button>
            </Link>
            
            <Link to="/login">
              <Button variant="outline" size="lg" className="px-8 py-6 text-lg group">
                View Demo
                <div className="ml-2 w-2 h-2 bg-primary rounded-full animate-pulse"></div>
              </Button>
            </Link>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mt-16 animate-fade-in" style={{ animationDelay: '1.2s' }}>
            <div className="text-center p-6 rounded-lg bg-card/50 backdrop-blur border hover:bg-card/80 transition-all duration-300 hover:scale-105">
              <div className="text-3xl font-bold text-primary mb-2">40%</div>
              <div className="text-muted-foreground">Average Conversion Increase</div>
            </div>
            <div className="text-center p-6 rounded-lg bg-card/50 backdrop-blur border hover:bg-card/80 transition-all duration-300 hover:scale-105">
              <div className="text-3xl font-bold text-primary mb-2">2.5x</div>
              <div className="text-muted-foreground">Faster Page Load Times</div>
            </div>
            <div className="text-center p-6 rounded-lg bg-card/50 backdrop-blur border hover:bg-card/80 transition-all duration-300 hover:scale-105">
              <div className="text-3xl font-bold text-primary mb-2">90%</div>
              <div className="text-muted-foreground">Customer Satisfaction</div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
