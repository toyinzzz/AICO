
import LandingHeader from "@/components/Landing/LandingHeader";
import AnimatedHeroSection from "@/components/Landing/AnimatedHeroSection";
import FeaturesSection from "@/components/Landing/FeaturesSection";
import BenefitsSection from "@/components/Landing/BenefitsSection";
import TestimonialsSection from "@/components/Landing/TestimonialsSection";
import CTASection from "@/components/Landing/CTASection";
import LandingFooter from "@/components/Landing/LandingFooter";

export default function Landing() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-background via-background to-primary/5">
      <LandingHeader />
      <AnimatedHeroSection />
      <FeaturesSection />
      <BenefitsSection />
      <TestimonialsSection />
      <CTASection />
      <LandingFooter />
    </div>
  );
}
