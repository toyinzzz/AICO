
import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { PieChart, Pie, Cell, ResponsiveContainer, Tooltip } from "recharts";
import { DollarSign, TrendingDown, AlertTriangle } from "lucide-react";

interface MoneyLostData {
  category: string;
  amount: number;
  color: string;
}

export function MoneyLostChart() {
  const [currentLoss, setCurrentLoss] = useState(0);
  const [lossData, setLossData] = useState<MoneyLostData[]>([
    { category: "Cart Abandonment", amount: 15420, color: "#dc2626" }, // Red - Critical
    { category: "Slow Loading", amount: 8750, color: "#ea580c" }, // Orange - High
    { category: "Poor Mobile UX", amount: 12300, color: "#d97706" }, // Amber - High
    { category: "Checkout Issues", amount: 9680, color: "#dc2626" }, // Red - Critical
    { category: "Missing Trust Signals", amount: 6420, color: "#ca8a04" } // Yellow - Medium
  ]);

  // Simulate real-time money loss
  useEffect(() => {
    const interval = setInterval(() => {
      const increment = Math.random() * 50 + 10; // Random increment between 10-60
      setCurrentLoss(prev => prev + increment);
      
      // Update data with small random variations
      setLossData(prev => prev.map(item => ({
        ...item,
        amount: item.amount + Math.random() * 20 - 10
      })));
    }, 2000);

    return () => clearInterval(interval);
  }, []);

  const totalLoss = lossData.reduce((sum, item) => sum + item.amount, 0) + currentLoss;

  const CustomTooltip = ({ active, payload }: any) => {
    if (active && payload && payload.length) {
      const data = payload[0].payload;
      return (
        <div className="bg-background border rounded-lg p-3 shadow-lg">
          <p className="font-medium">{data.category}</p>
          <p className="text-red-600 font-bold">
            ${data.amount.toLocaleString()}
          </p>
        </div>
      );
    }
    return null;
  };

  return (
    <Card className="hover:shadow-lg transition-shadow duration-300">
      <CardHeader className="pb-4">
        <CardTitle className="flex items-center gap-2 text-red-600 text-lg">
          <AlertTriangle className="w-4 h-4" />
          Revenue Loss Analytics
        </CardTitle>
        <CardDescription className="text-sm">
          Real-time revenue loss tracking due to optimization opportunities
        </CardDescription>
      </CardHeader>
      <CardContent className="pt-0">
        <div className="space-y-4">
          {/* Live Counter */}
          <div className="text-center space-y-1">
            <div className="text-2xl font-bold text-red-600 animate-pulse">
              ${totalLoss.toLocaleString()}
            </div>
            <div className="flex items-center justify-center gap-1 text-xs text-muted-foreground">
              <TrendingDown className="w-3 h-3 text-red-500" />
              Potential loss this month
            </div>
          </div>

          {/* Circular Chart */}
          <div className="h-48">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={lossData}
                  cx="50%"
                  cy="50%"
                  innerRadius={40}
                  outerRadius={80}
                  paddingAngle={2}
                  dataKey="amount"
                >
                  {lossData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip content={<CustomTooltip />} />
              </PieChart>
            </ResponsiveContainer>
          </div>

          {/* Legend */}
          <div className="space-y-1">
            {lossData.map((item, index) => (
              <div key={index} className="flex items-center justify-between text-xs">
                <div className="flex items-center gap-2">
                  <div 
                    className="w-2 h-2 rounded-full" 
                    style={{ backgroundColor: item.color }}
                  />
                  <span className="truncate">{item.category}</span>
                </div>
                <span className="font-medium text-red-600 ml-2">
                  ${item.amount.toLocaleString()}
                </span>
              </div>
            ))}
          </div>

          {/* Real-time indicator */}
          <div className="flex items-center justify-center gap-2 text-xs text-muted-foreground pt-2">
            <div className="w-1.5 h-1.5 bg-red-500 rounded-full animate-pulse" />
            Updates every 2 seconds
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
