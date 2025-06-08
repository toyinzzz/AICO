
import { Button } from "@/components/ui/button";
import { Progress } from "@/components/ui/progress";
import { Search, Zap } from "lucide-react";

interface AnalysisProgressProps {
  isAnalyzing: boolean;
  analysisProgress: number;
  onRunAnalysis: () => void;
  canAnalyze: boolean;
}

export function AnalysisProgress({ 
  isAnalyzing, 
  analysisProgress, 
  onRunAnalysis, 
  canAnalyze 
}: AnalysisProgressProps) {
  const handleRunAnalysis = () => {
    console.log("Button clicked - canAnalyze:", canAnalyze, "isAnalyzing:", isAnalyzing);
    if (!isAnalyzing && canAnalyze) {
      console.log("Calling onRunAnalysis function");
      onRunAnalysis();
    } else {
      console.log("Analysis blocked - either already analyzing or can't analyze");
    }
  };

  console.log("AnalysisProgress rendered - canAnalyze:", canAnalyze, "isAnalyzing:", isAnalyzing);

  return (
    <div className="space-y-4">
      <Button 
        onClick={handleRunAnalysis} 
        disabled={isAnalyzing || !canAnalyze}
        className="w-full"
      >
        {isAnalyzing ? (
          <>
            <Zap className="w-4 h-4 mr-2 animate-spin" />
            Analyzing...
          </>
        ) : (
          <>
            <Search className="w-4 h-4 mr-2" />
            Run Competitive Analysis
          </>
        )}
      </Button>

      {isAnalyzing && (
        <div className="space-y-2">
          <Progress value={analysisProgress} className="h-2" />
          <p className="text-sm text-muted-foreground">
            {analysisProgress < 30 && "Crawling websites..."}
            {analysisProgress >= 30 && analysisProgress < 60 && "Analyzing performance metrics..."}
            {analysisProgress >= 60 && analysisProgress < 90 && "Comparing conversion elements..."}
            {analysisProgress >= 90 && "Generating insights..."}
          </p>
        </div>
      )}
    </div>
  );
}
