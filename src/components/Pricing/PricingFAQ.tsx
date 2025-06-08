
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function PricingFAQ() {
  return (
    <section className="container mx-auto px-4 py-20 bg-gradient-to-r from-blue-50/50 to-purple-50/50 dark:from-gray-800/50 dark:to-gray-700/50">
      <div className="text-center mb-12">
        <h2 className="text-3xl font-bold mb-4">Frequently Asked Questions</h2>
      </div>
      
      <div className="max-w-3xl mx-auto grid gap-6">
        <Card>
          <CardHeader>
            <CardTitle className="text-lg">Can I change plans anytime?</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-muted-foreground">
              Yes, you can upgrade or downgrade your plan at any time. Changes take effect immediately, 
              and we'll prorate any charges or credits.
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="text-lg">What happens after my free trial?</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-muted-foreground">
              After your 14-day free trial, you'll be automatically enrolled in the plan you selected. 
              You can cancel anytime during the trial without being charged.
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="text-lg">Do you offer custom enterprise solutions?</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-muted-foreground">
              Yes, we offer custom enterprise solutions with dedicated support, custom integrations, 
              and tailored AI models. Contact our sales team to discuss your specific needs.
            </p>
          </CardContent>
        </Card>
      </div>
    </section>
  );
}
