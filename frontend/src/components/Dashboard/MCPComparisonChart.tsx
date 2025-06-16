import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Cell } from "recharts";
import { TrendingUp, DollarSign, Target, Loader2 } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { useMCPData } from "@/hooks/useMCPData";
import { MCPVariantData } from "@/lib/api/mcpService";

interface ChartMCPData extends MCPVariantData {
  color: string;
}

export function MCPComparisonChart() {
  // Fetch MCP data using the hook
  const { data, loading, error, refresh } = useMCPData({
    refreshInterval: 30000, // Refresh every 30 seconds
    mockData: true // Use mock data until backend is ready
  });

  // Define colors for variants
  const variantColors = [
    "#94a3b8", // Control - slate
    "#3b82f6", // Variant A - blue
    "#8b5cf6", // Variant B - purple
    "#ef4444", // Variant C - red
    "#10b981", // Variant D - green
    "#f59e0b", // Variant E - amber
  ];

  // Transform API data to chart data with colors
  const chartData: ChartMCPData[] = data?.variants.map((variant, index) => ({
    ...variant,
    color: variant.isControl ? variantColors[0] : variantColors[(index % (variantColors.length - 1)) + 1]
  })) || [];

  const CustomTooltip = ({ active, payload }: any) => {
    if (active && payload && payload.length) {
      const data = payload[0].payload as MCPData;
      return (
        <div className="bg-background border rounded-lg p-3 shadow-lg">
          <p className="font-medium">{data.variantName}</p>
          <p className="font-bold">
            MCP: {data.mcpValue.toFixed(2)}%
          </p>
          <div className="text-sm text-muted-foreground mt-1">
            <p>Revenue: ${data.revenue.toLocaleString()}</p>
            <p>Conversions: {data.conversions.toLocaleString()}</p>
            <p>Revenue/Conv: ${data.revenuePerConversion.toFixed(2)}</p>
          </div>
        </div>
      );
    }
    return null;
  };

  // Find the best performing variant
  const bestVariant = chartData.length > 0 ? 
    chartData.reduce((best, current) => {
      return (current.mcpValue > best.mcpValue) ? current : best;
    }, chartData[0]) : 
    null;

  return (
    <Card className="hover:shadow-lg transition-shadow duration-300">
      <CardHeader className="pb-4">
        <CardTitle className="flex items-center gap-2 text-lg">
          <Target className="w-4 h-4 text-primary" />
          MCP Analysis
        </CardTitle>
        <CardDescription className="text-sm">
          Marginal Contribution to Profit by variant
        </CardDescription>
      </CardHeader>
      <CardContent className="pt-0">
        {loading ? (
          <div className="flex items-center justify-center h-64">
            <Loader2 className="w-8 h-8 text-primary animate-spin" />
          </div>
        ) : error ? (
          <div className="text-center text-red-500 p-4">
            Error loading MCP data. Please try again.
          </div>
        ) : chartData.length === 0 ? (
          <div className="text-center text-muted-foreground p-4">
            No MCP data available. Start an A/B test to see results.
          </div>
        ) : (
          <div className="space-y-4">
            {/* Test Info */}
            {data && (
              <div className="text-sm text-muted-foreground">
                <span className="font-medium">{data.testName}</span> · Started {new Date(data.startDate).toLocaleDateString()}
              </div>
            )}
            
            {/* Best Performer Highlight */}
            {bestVariant && bestVariant.mcpValue > 0 && (
              <div className="bg-green-50 dark:bg-green-950/30 border border-green-200 dark:border-green-900 rounded-lg p-3">
                <div className="flex items-center gap-2">
                  <TrendingUp className="w-4 h-4 text-green-600" />
                  <span className="font-medium text-green-700 dark:text-green-400">Best Performer: {bestVariant.variantName}</span>
                </div>
                <div className="mt-1 text-sm text-green-600 dark:text-green-500">
                  {bestVariant.mcpValue.toFixed(2)}% higher profit per conversion
                </div>
              </div>
            )}

            {/* MCP Bar Chart */}
            <div className="h-64">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart
                  data={chartData}
                  margin={{ top: 20, right: 30, left: 20, bottom: 5 }}
                >
                  <CartesianGrid strokeDasharray="3 3" opacity={0.2} />
                  <XAxis dataKey="variantName" />
                  <YAxis 
                    label={{ 
                      value: 'MCP %', 
                      angle: -90, 
                      position: 'insideLeft',
                      style: { textAnchor: 'middle' }
                    }} 
                  />
                  <Tooltip content={<CustomTooltip />} />
                  <Bar dataKey="mcpValue">
                    {chartData.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.color} />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            </div>

            {/* Variant Comparison Table */}
            <div className="space-y-2">
              <div className="text-sm font-medium">Variant Comparison</div>
              <div className="space-y-2">
                {chartData.map((variant, index) => (
                  <div key={index} className="flex items-center justify-between text-sm border-b pb-2">
                    <div className="flex items-center gap-2">
                      <div 
                        className="w-3 h-3 rounded-full" 
                        style={{ backgroundColor: variant.color }}
                      />
                      <span>{variant.variantName}</span>
                      {variant.isControl && (
                        <Badge variant="outline" className="ml-1 text-xs">Control</Badge>
                      )}
                      {variant.isStatisticallySignificant && variant.mcpValue !== 0 && (
                        <Badge variant="secondary" className="ml-1 text-xs">Significant</Badge>
                      )}
                    </div>
                    <div className="flex items-center gap-3">
                      <span className="text-muted-foreground">
                        ${variant.revenuePerConversion.toFixed(2)}/conv
                      </span>
                      <span className={`font-medium ${variant.mcpValue > 0 ? 'text-green-600' : variant.mcpValue < 0 ? 'text-red-600' : 'text-muted-foreground'}`}>
                        {variant.mcpValue > 0 ? '+' : ''}{variant.mcpValue.toFixed(2)}%
                      </span>
                    </div>
                  </div>
                ))}
              </div>
            </div>

            {/* Real-time indicator */}
            <div className="flex items-center justify-center gap-2 text-xs text-muted-foreground pt-2">
              <div className="w-1.5 h-1.5 bg-primary rounded-full animate-pulse" />
              Updates every 30 seconds
            </div>
          </div>
        )}
      </CardContent>
    </Card>
  );
}