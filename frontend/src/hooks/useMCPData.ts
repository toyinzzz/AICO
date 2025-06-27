import { useState, useEffect } from 'react';
import mcpService, { MCPAnalysisResult, MCPVariantData } from '@/lib/api/mcpService';

interface UseMCPDataOptions {
  testId?: string;
  refreshInterval?: number;
  mockData?: boolean; // For development without backend
}

interface UseMCPDataResult {
  data: MCPAnalysisResult | null;
  loading: boolean;
  error: Error | null;
  refresh: () => Promise<void>;
}

// Mock data for development without backend
const generateMockMCPData = (): MCPAnalysisResult => {
  const variants: MCPVariantData[] = [
    { 
      variantId: 'control',
      variantName: 'Control', 
      mcpValue: 0, 
      revenue: 10000, 
      conversions: 1000, 
      revenuePerConversion: 10,
      isControl: true,
      isStatisticallySignificant: true
    },
    { 
      variantId: 'variant-a',
      variantName: 'Variant A', 
      mcpValue: 9.09, 
      revenue: 12000, 
      conversions: 1100, 
      revenuePerConversion: 10.91,
      isControl: false,
      isStatisticallySignificant: true
    },
    { 
      variantId: 'variant-b',
      variantName: 'Variant B', 
      mcpValue: 15.5, 
      revenue: 11500, 
      conversions: 1000, 
      revenuePerConversion: 11.55,
      isControl: false,
      isStatisticallySignificant: false
    },
    { 
      variantId: 'variant-c',
      variantName: 'Variant C', 
      mcpValue: -5.2, 
      revenue: 9000, 
      conversions: 950, 
      revenuePerConversion: 9.48,
      isControl: false,
      isStatisticallySignificant: true
    },
  ];

  return {
    testId: 'mock-test-1',
    testName: 'Homepage Headline Test',
    variants,
    bestVariantId: 'variant-b',
    controlVariantId: 'control',
    startDate: new Date(Date.now() - 30 * 24 * 60 * 60 * 1000).toISOString(), // 30 days ago
    endDate: new Date().toISOString(),
    totalRevenue: variants.reduce((sum, variant) => sum + variant.revenue, 0),
    totalConversions: variants.reduce((sum, variant) => sum + variant.conversions, 0)
  };
};

/**
 * Hook to fetch and manage MCP data
 */
export function useMCPData(options: UseMCPDataOptions = {}): UseMCPDataResult {
  const { testId, refreshInterval = 0, mockData = true } = options;
  const [data, setData] = useState<MCPAnalysisResult | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<Error | null>(null);

  const fetchData = async () => {
    try {
      setLoading(true);
      setError(null);

      let result: MCPAnalysisResult;

      if (mockData) {
        // Use mock data for development
        result = generateMockMCPData();
        // Simulate API delay
        await new Promise(resolve => setTimeout(resolve, 500));
      } else {
        // Fetch real data from API
        if (testId) {
          result = await mcpService.getMCPAnalysis(testId);
        } else {
          // If no testId is provided, get the first active test
          const activeTests = await mcpService.getAllActiveMCPAnalyses();
          if (activeTests.length === 0) {
            throw new Error('No active tests found');
          }
          result = activeTests[0];
        }
      }

      setData(result);
    } catch (err) {
      setError(err instanceof Error ? err : new Error('Unknown error occurred'));
    } finally {
      setLoading(false);
    }
  };

  // Initial fetch
  useEffect(() => {
    fetchData();

    // Set up refresh interval if specified
    if (refreshInterval > 0) {
      const intervalId = setInterval(fetchData, refreshInterval);
      return () => clearInterval(intervalId);
    }
  }, [testId, refreshInterval, mockData]);

  return {
    data,
    loading,
    error,
    refresh: fetchData
  };
}