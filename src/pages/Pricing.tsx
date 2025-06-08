
import PricingHeader from "@/components/Pricing/PricingHeader";
import PricingHero from "@/components/Pricing/PricingHero";
import PricingPlans from "@/components/Pricing/PricingPlans";
import PricingFAQ from "@/components/Pricing/PricingFAQ";
import LandingFooter from "@/components/Landing/LandingFooter";

export default function Pricing() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-background via-background to-primary/5">
      <PricingHeader />
      <PricingHero />
      <PricingPlans />
      <PricingFAQ />
      <LandingFooter />
    </div>
  );
}
