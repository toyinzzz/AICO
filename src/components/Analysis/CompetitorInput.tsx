
import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { useToast } from "@/hooks/use-toast";
import { Globe, Plus, X } from "lucide-react";

interface CompetitorSite {
  id: string;
  url: string;
  name: string;
  isYourSite?: boolean;
}

interface CompetitorInputProps {
  competitorSites: CompetitorSite[];
  onCompetitorSitesChange: (sites: CompetitorSite[]) => void;
}

export function CompetitorInput({ competitorSites, onCompetitorSitesChange }: CompetitorInputProps) {
  const { toast } = useToast();
  const [newCompetitorUrl, setNewCompetitorUrl] = useState("");
  const [newCompetitorName, setNewCompetitorName] = useState("");

  const addCompetitor = () => {
    if (!newCompetitorUrl.trim()) {
      toast({
        title: "URL Required",
        description: "Please enter a competitor URL.",
        variant: "destructive",
      });
      return;
    }

    if (competitorSites.length >= 4) {
      toast({
        title: "Maximum Reached",
        description: "You can analyze up to 3 competitors at once.",
        variant: "destructive",
      });
      return;
    }

    try {
      new URL(newCompetitorUrl.startsWith('http') ? newCompetitorUrl : `https://${newCompetitorUrl}`);
    } catch {
      toast({
        title: "Invalid URL",
        description: "Please enter a valid URL.",
        variant: "destructive",
      });
      return;
    }

    const newCompetitor: CompetitorSite = {
      id: `comp-${Date.now()}`,
      url: newCompetitorUrl,
      name: newCompetitorName || newCompetitorUrl.replace(/^https?:\/\//, '').split('.')[0],
    };

    onCompetitorSitesChange([...competitorSites, newCompetitor]);
    setNewCompetitorUrl("");
    setNewCompetitorName("");
  };

  const removeCompetitor = (id: string) => {
    onCompetitorSitesChange(competitorSites.filter(site => site.id !== id));
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center space-x-2">
          <Globe className="w-5 h-5" />
          <span>Competitor Websites</span>
        </CardTitle>
        <CardDescription>
          Add up to 3 competitor websites to analyze against your site
        </CardDescription>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="space-y-2">
          {competitorSites.map((site) => (
            <div key={site.id} className="flex items-center justify-between p-3 border rounded-lg">
              <div className="flex items-center space-x-3">
                <Globe className="w-4 h-4 text-muted-foreground" />
                <div>
                  <p className="font-medium">{site.name}</p>
                  <p className="text-sm text-muted-foreground">{site.url}</p>
                </div>
                {site.isYourSite && <Badge variant="outline">Your Site</Badge>}
              </div>
              {!site.isYourSite && (
                <Button
                  variant="ghost"
                  size="sm"
                  onClick={() => removeCompetitor(site.id)}
                >
                  <X className="w-4 h-4" />
                </Button>
              )}
            </div>
          ))}
        </div>

        {competitorSites.length < 4 && (
          <div className="flex space-x-2">
            <Input
              placeholder="https://competitor.com"
              value={newCompetitorUrl}
              onChange={(e) => setNewCompetitorUrl(e.target.value)}
              className="flex-1"
            />
            <Input
              placeholder="Competitor Name (optional)"
              value={newCompetitorName}
              onChange={(e) => setNewCompetitorName(e.target.value)}
              className="flex-1"
            />
            <Button onClick={addCompetitor}>
              <Plus className="w-4 h-4 mr-2" />
              Add
            </Button>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
