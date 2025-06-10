
import { CompetitorAnalysis } from "@/components/Analysis/CompetitorAnalysis";

export default function CompetitorAnalysisPage() {
  return (
    <div className="flex-1 bg-gradient-to-br from-background via-background to-muted/20">
      <main className="flex-1 overflow-y-auto p-8">
        <div className="space-y-8 max-w-7xl mx-auto">
          <div className="space-y-2">
            <h1 className="text-4xl font-bold bg-gradient-to-r from-primary to-primary/60 bg-clip-text text-transparent">
              Competitor Analysis
            </h1>
            <p className="text-muted-foreground text-lg">
              Analyze competitor websites to identify opportunities and benchmark performance
            </p>
          </div>
          <CompetitorAnalysis />
        </div>
      </main>
    </div>
  );
}
