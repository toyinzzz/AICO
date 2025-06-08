
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { TrendingUp, TrendingDown, ArrowRight, CheckCircle } from "lucide-react";

interface ComparisonData {
  metric: string;
  before: number;
  after: number;
  unit: string;
  improvement: number;
  isPercentage?: boolean;
}

const comparisonData: ComparisonData[] = [
  {
    metric: "Conversion Rate",
    before: 1.8,
    after: 2.4,
    unit: "%",
    improvement: 33.3,
    isPercentage: true
  },
  {
    metric: "Bounce Rate", 
    before: 68,
    after: 45,
    unit: "%",
    improvement: -33.8,
    isPercentage: true
  },
  {
    metric: "Average Session Duration",
    before: 2.1,
    after: 3.4,
    unit: "min",
    improvement: 61.9,
    isPercentage: false
  },
  {
    metric: "Revenue per Visitor",
    before: 8.12,
    after: 12.45,
    unit: "$",
    improvement: 53.3,
    isPercentage: false
  },
  {
    metric: "Cart Abandonment Rate",
    before: 72,
    after: 58,
    unit: "%", 
    improvement: -19.4,
    isPercentage: true
  },
  {
    metric: "Page Load Speed",
    before: 4.2,
    after: 2.1,
    unit: "s",
    improvement: -50.0,
    isPercentage: false
  }
];

export function BeforeAfterComparison() {
  const formatValue = (value: number, unit: string) => {
    if (unit === "$") {
      return `$${value.toFixed(2)}`;
    }
    return `${value}${unit}`;
  };

  const getImprovementColor = (improvement: number) => {
    return improvement > 0 ? "text-green-600" : "text-red-600";
  };

  const getImprovementIcon = (improvement: number) => {
    return improvement > 0 ? <TrendingUp className="w-4 h-4" /> : <TrendingDown className="w-4 h-4" />;
  };

  return (
    <Card className="card-hover">
      <CardHeader>
        <div className="flex items-center justify-between">
          <div>
            <CardTitle className="flex items-center space-x-2">
              <CheckCircle className="w-5 h-5 text-green-500" />
              <span>Before vs After Analysis</span>
            </CardTitle>
            <CardDescription>
              Performance comparison after implementing AI recommendations
            </CardDescription>
          </div>
          <Badge className="bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200">
            6 Optimizations Applied
          </Badge>
        </div>
      </CardHeader>
      <CardContent>
        <div className="space-y-6">
          {comparisonData.map((data, index) => (
            <div key={index} className="grid grid-cols-1 md:grid-cols-3 gap-4 p-4 border rounded-lg bg-gradient-to-r from-gray-50 to-white dark:from-gray-900 dark:to-gray-800 metric-glow">
              {/* Metric Name */}
              <div className="flex flex-col justify-center">
                <h3 className="font-semibold text-lg">{data.metric}</h3>
                <div className={`flex items-center space-x-1 text-sm ${getImprovementColor(data.improvement)}`}>
                  {getImprovementIcon(data.improvement)}
                  <span>
                    {Math.abs(data.improvement).toFixed(1)}% {data.improvement > 0 ? 'improvement' : 'reduction'}
                  </span>
                </div>
              </div>

              {/* Before vs After Values */}
              <div className="grid grid-cols-3 gap-2 items-center">
                {/* Before */}
                <div className="text-center p-3 bg-red-50 dark:bg-red-950/20 rounded-lg border border-red-200 dark:border-red-800 interactive-gloss">
                  <p className="text-xs text-muted-foreground mb-1">Before</p>
                  <p className="text-lg font-bold text-red-600">{formatValue(data.before, data.unit)}</p>
                </div>

                {/* Arrow */}
                <div className="flex justify-center">
                  <ArrowRight className="w-5 h-5 text-muted-foreground" />
                </div>

                {/* After */}
                <div className="text-center p-3 bg-green-50 dark:bg-green-950/20 rounded-lg border border-green-200 dark:border-green-800 interactive-gloss">
                  <p className="text-xs text-muted-foreground mb-1">After</p>
                  <p className="text-lg font-bold text-green-600">{formatValue(data.after, data.unit)}</p>
                </div>
              </div>

              {/* Progress Bar */}
              <div className="flex flex-col justify-center">
                <div className="mb-2">
                  <div className="flex justify-between text-xs text-muted-foreground mb-1">
                    <span>Progress</span>
                    <span>{Math.abs(data.improvement).toFixed(1)}%</span>
                  </div>
                  <Progress 
                    value={Math.min(Math.abs(data.improvement), 100)} 
                    className="h-2"
                  />
                </div>
              </div>
            </div>
          ))}

          {/* Summary Card */}
          <div className="mt-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 dark:from-blue-950/20 dark:to-indigo-950/20 rounded-lg border border-blue-200 dark:border-blue-800 card-hover">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4 text-center">
              <div>
                <p className="text-2xl font-bold text-blue-600">+$47,230</p>
                <p className="text-sm text-muted-foreground">Additional Monthly Revenue</p>
              </div>
              <div>
                <p className="text-2xl font-bold text-green-600">+33.3%</p>
                <p className="text-sm text-muted-foreground">Avg. Conversion Improvement</p>
              </div>
              <div>
                <p className="text-2xl font-bold text-purple-600">2.3x</p>
                <p className="text-sm text-muted-foreground">ROI on Optimizations</p>
              </div>
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
