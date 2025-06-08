
import { Link } from "react-router-dom";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Check, ArrowRight } from "lucide-react";

interface PricingCardProps {
  plan: {
    name: string;
    price: number;
    description: string;
    features: string[];
    buttonText: string;
    buttonVariant: "default" | "outline";
  };
}

export default function PricingCard({ plan }: PricingCardProps) {
  return (
    <Card className="transition-all duration-300 hover:shadow-lg hover:scale-105 h-full flex flex-col">
      <CardHeader className="text-center pb-4">
        <CardTitle className="text-2xl font-bold">{plan.name}</CardTitle>
        <CardDescription className="text-base">{plan.description}</CardDescription>
        <div className="mt-4">
          <div className="flex items-baseline justify-center">
            <span className="text-5xl font-bold">${plan.price}</span>
            <span className="text-xl text-muted-foreground ml-1">/month</span>
          </div>
          <p className="text-sm text-muted-foreground mt-2">Billed monthly</p>
        </div>
      </CardHeader>

      <CardContent className="flex flex-col flex-1">
        <ul className="space-y-3 flex-1 mb-6">
          {plan.features.map((feature, featureIndex) => (
            <li key={featureIndex} className="flex items-start space-x-3">
              <Check className="w-5 h-5 text-green-500 flex-shrink-0 mt-0.5" />
              <span className="text-sm">{feature}</span>
            </li>
          ))}
        </ul>

        <div className="mt-auto">
          {plan.name === "Agency" ? (
            <Link to="/contact">
              <Button 
                variant={plan.buttonVariant} 
                className="w-full text-base py-6"
                size="lg"
              >
                {plan.buttonText}
                <ArrowRight className="ml-2 w-4 h-4" />
              </Button>
            </Link>
          ) : (
            <Link to="/signup">
              <Button 
                variant={plan.buttonVariant} 
                className="w-full text-base py-6"
                size="lg"
              >
                {plan.buttonText}
                <ArrowRight className="ml-2 w-4 h-4" />
              </Button>
            </Link>
          )}
        </div>
      </CardContent>
    </Card>
  );
}
