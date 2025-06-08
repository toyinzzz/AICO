
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Clock, Smartphone, MousePointer } from "lucide-react";

interface CompetitorData {
  id: string;
  url: string;
  name: string;
  isYourSite?: boolean;
  metrics: {
    seoScore: number;
    pageSpeed: number;
    mobileScore: number;
    uxScore: number;
    contentScore: number;
    ctaCount: number;
    loadTime: number;
    conversionRate: number;
    trustSignals: number;
  };
  strengths: string[];
  weaknesses: string[];
  recommendations: string[];
}

interface MetricsTableProps {
  analysisData: CompetitorData[];
}

export function MetricsTable({ analysisData }: MetricsTableProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Detailed Metrics Comparison</CardTitle>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Metric</TableHead>
              {analysisData.map(site => (
                <TableHead key={site.id}>
                  {site.name}
                  {site.isYourSite && <Badge variant="outline" className="ml-2">You</Badge>}
                </TableHead>
              ))}
            </TableRow>
          </TableHeader>
          <TableBody>
            <TableRow>
              <TableCell className="font-medium">SEO Score</TableCell>
              {analysisData.map(site => (
                <TableCell key={site.id}>
                  <div className="flex items-center space-x-2">
                    <span>{site.metrics.seoScore}</span>
                    <Progress value={site.metrics.seoScore} className="w-16 h-2" />
                  </div>
                </TableCell>
              ))}
            </TableRow>
            <TableRow>
              <TableCell className="font-medium">Page Speed</TableCell>
              {analysisData.map(site => (
                <TableCell key={site.id}>
                  <div className="flex items-center space-x-2">
                    <Clock className="w-4 h-4" />
                    <span>{site.metrics.loadTime}s</span>
                  </div>
                </TableCell>
              ))}
            </TableRow>
            <TableRow>
              <TableCell className="font-medium">Mobile Score</TableCell>
              {analysisData.map(site => (
                <TableCell key={site.id}>
                  <div className="flex items-center space-x-2">
                    <Smartphone className="w-4 h-4" />
                    <span>{site.metrics.mobileScore}</span>
                  </div>
                </TableCell>
              ))}
            </TableRow>
            <TableRow>
              <TableCell className="font-medium">CTA Count</TableCell>
              {analysisData.map(site => (
                <TableCell key={site.id}>
                  <div className="flex items-center space-x-2">
                    <MousePointer className="w-4 h-4" />
                    <span>{site.metrics.ctaCount}</span>
                  </div>
                </TableCell>
              ))}
            </TableRow>
            <TableRow>
              <TableCell className="font-medium">Conversion Rate</TableCell>
              {analysisData.map(site => (
                <TableCell key={site.id}>
                  <span>{site.metrics.conversionRate}%</span>
                </TableCell>
              ))}
            </TableRow>
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
}
