
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from "recharts";

const data = [
  { name: 'Jan', conversionRate: 1.2, revenue: 85000, bounceRate: 68 },
  { name: 'Feb', conversionRate: 1.4, revenue: 92000, bounceRate: 65 },
  { name: 'Mar', conversionRate: 1.6, revenue: 105000, bounceRate: 62 },
  { name: 'Apr', conversionRate: 1.8, revenue: 118000, bounceRate: 58 },
  { name: 'May', conversionRate: 1.9, revenue: 125000, bounceRate: 55 },
  { name: 'Jun', conversionRate: 2.1, revenue: 145000, bounceRate: 52 },
];

export function MetricsChart() {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Performance Trends</CardTitle>
        <CardDescription>Conversion rate and revenue over time</CardDescription>
      </CardHeader>
      <CardContent>
        <ResponsiveContainer width="100%" height={300}>
          <LineChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis yAxisId="left" />
            <YAxis yAxisId="right" orientation="right" />
            <Tooltip />
            <Legend />
            <Line
              yAxisId="left"
              type="monotone"
              dataKey="conversionRate"
              stroke="#8884d8"
              strokeWidth={2}
              name="Conversion Rate (%)"
            />
            <Line
              yAxisId="right"
              type="monotone"
              dataKey="revenue"
              stroke="#82ca9d"
              strokeWidth={2}
              name="Revenue ($)"
            />
          </LineChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
