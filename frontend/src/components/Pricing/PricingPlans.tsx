
import { Check } from "lucide-react";
import PricingCard from "./PricingCard";

export default function PricingPlans() {
  const plans = [
    {
      name: "Starter",
      price: 29,
      description: "Perfect for small businesses and personal websites",
      features: [
        "1 website analysis",
        "Basic AI recommendations",
        "Weekly performance reports",
        "Email support",
        "Dashboard access"
      ],
      buttonText: "Start Free Trial",
      buttonVariant: "outline" as const
    },
    {
      name: "Pro",
      price: 99,
      description: "Ideal for growing businesses and marketing teams",
      features: [
        "5 website analyses",
        "Advanced AI recommendations",
        "Real-time analytics",
        "A/B testing suggestions",
        "Priority support",
        "Custom reports",
        "Integration support"
      ],
      buttonText: "Start Free Trial",
      buttonVariant: "default" as const
    },
    {
      name: "Agency",
      price: 299,
      description: "For agencies and enterprises managing multiple clients",
      features: [
        "Unlimited website analyses",
        "White-label reports",
        "Advanced competitor analysis",
        "Custom AI model training",
        "24/7 dedicated support",
        "API access",
        "Multi-user collaboration",
        "Custom integrations"
      ],
      buttonText: "Contact Sales",
      buttonVariant: "outline" as const
    }
  ];

  return (
    <section className="container mx-auto px-4 pb-20 pt-6">
      <div className="grid md:grid-cols-3 gap-8 max-w-6xl mx-auto items-stretch">
        {plans.map((plan) => (
          <PricingCard key={plan.name} plan={plan} />
        ))}
      </div>

      {/* Additional Info */}
      <div className="text-center mt-16 max-w-2xl mx-auto">
        <p className="text-muted-foreground mb-6">
          All plans include a 14-day free trial. No setup fees, cancel anytime.
        </p>
        <div className="flex flex-wrap justify-center gap-6 text-sm text-muted-foreground">
          <div className="flex items-center space-x-2">
            <Check className="w-4 h-4 text-green-500" />
            <span>14-day free trial</span>
          </div>
          <div className="flex items-center space-x-2">
            <Check className="w-4 h-4 text-green-500" />
            <span>No setup fees</span>
          </div>
          <div className="flex items-center space-x-2">
            <Check className="w-4 h-4 text-green-500" />
            <span>Cancel anytime</span>
          </div>
          <div className="flex items-center space-x-2">
            <Check className="w-4 h-4 text-green-500" />
            <span>24/7 support</span>
          </div>
        </div>
      </div>
    </section>
  );
}
