
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { CheckCircle, Clock, AlertTriangle } from "lucide-react";

const recommendations = [
  {
    id: "1",
    title: "Add trust badges near checkout",
    impact: "High",
    expectedUplift: "+15%",
    status: "pending",
    site: "E-commerce Store"
  },
  {
    id: "2", 
    title: "Optimize mobile checkout flow",
    impact: "High",
    expectedUplift: "+22%",
    status: "pending",
    site: "E-commerce Store"
  },
  {
    id: "3",
    title: "A/B test CTA button colors",
    impact: "Medium",
    expectedUplift: "+8%",
    status: "implemented",
    site: "SaaS Landing"
  },
  {
    id: "4",
    title: "Reduce form fields",
    impact: "Medium", 
    expectedUplift: "+12%",
    status: "pending",
    site: "SaaS Landing"
  }
];

export function RecommendationsList() {
  const getStatusIcon = (status: string) => {
    switch (status) {
      case "implemented":
        return <CheckCircle className="w-4 h-4 text-green-500" />;
      case "pending":
        return <Clock className="w-4 h-4 text-yellow-500" />;
      default:
        return <AlertTriangle className="w-4 h-4 text-red-500" />;
    }
  };

  const getImpactColor = (impact: string) => {
    switch (impact.toLowerCase()) {
      case "high":
        return "destructive";
      case "medium":
        return "default";
      case "low":
        return "secondary";
      default:
        return "outline";
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Recent Recommendations</CardTitle>
        <CardDescription>AI-generated CRO suggestions</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          {recommendations.map((rec) => (
            <div key={rec.id} className="flex items-center justify-between p-3 border rounded-lg">
              <div className="flex items-center space-x-3">
                {getStatusIcon(rec.status)}
                <div>
                  <p className="font-medium text-sm">{rec.title}</p>
                  <p className="text-xs text-muted-foreground">{rec.site}</p>
                </div>
              </div>
              <div className="flex items-center space-x-2">
                <Badge variant={getImpactColor(rec.impact)}>{rec.impact}</Badge>
                <span className="text-sm font-medium text-green-600">{rec.expectedUplift}</span>
              </div>
            </div>
          ))}
          <Button variant="outline" className="w-full mt-4">
            View All Recommendations
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
