
import { Link } from "react-router-dom";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { ArrowRight } from "lucide-react";

export default function CTASection() {
  return (
    <section className="container mx-auto px-4 py-20">
      <Card className="text-center p-12 bg-gradient-to-r from-blue-500 to-purple-600 text-white">
        <CardContent>
          <h2 className="text-4xl font-bold mb-4">Ready to Optimize Your Website?</h2>
          <p className="text-xl mb-8 opacity-90">
            Join thousands of businesses already using AICO to boost their conversion rates.
          </p>
          <Link to="/signup">
            <Button size="lg" variant="secondary" className="text-lg px-8 py-6">
              Start Your Free Trial
              <ArrowRight className="ml-2 w-5 h-5" />
            </Button>
          </Link>
        </CardContent>
      </Card>
    </section>
  );
}
