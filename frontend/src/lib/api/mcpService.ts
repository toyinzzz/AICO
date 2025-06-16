import axios from 'axios';

// Define the base URL for API calls
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api';

// Define types for MCP data
export interface MCPVariantData {
  variantId: string;
  variantName: string;
  revenue: number;
  conversions: number;
  revenuePerConversion: number;
  mcpValue: number;
  isControl: boolean;
  isStatisticallySignificant: boolean;
}

export interface MCPAnalysisResult {
  testId: string;
  testName: string;
  variants: MCPVariantData[];
  bestVariantId: string | null;
  controlVariantId: string;
  startDate: string;
  endDate: string;
  totalRevenue: number;
  totalConversions: number;
}

// MCP Service for API calls
const mcpService = {
  /**
   * Get MCP analysis for a specific test
   * @param testId The ID of the test to analyze
   */
  getMCPAnalysis: async (testId: string): Promise<MCPAnalysisResult> => {
    try {
      const response = await axios.get(`${API_BASE_URL}/profit-tracking/mcp/${testId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching MCP analysis:', error);
      throw error;
    }
  },

  /**
   * Get MCP analysis for all active tests
   */
  getAllActiveMCPAnalyses: async (): Promise<MCPAnalysisResult[]> => {
    try {
      const response = await axios.get(`${API_BASE_URL}/profit-tracking/mcp/active`);
      return response.data;
    } catch (error) {
      console.error('Error fetching active MCP analyses:', error);
      throw error;
    }
  },

  /**
   * Get historical MCP data for a test over time
   * @param testId The ID of the test
   * @param startDate Optional start date for the time range
   * @param endDate Optional end date for the time range
   */
  getMCPHistory: async (
    testId: string,
    startDate?: string,
    endDate?: string
  ): Promise<{ date: string; variants: MCPVariantData[] }[]> => {
    try {
      let url = `${API_BASE_URL}/profit-tracking/mcp/${testId}/history`;
      
      // Add query parameters if provided
      const params = new URLSearchParams();
      if (startDate) params.append('startDate', startDate);
      if (endDate) params.append('endDate', endDate);
      
      if (params.toString()) {
        url += `?${params.toString()}`;
      }
      
      const response = await axios.get(url);
      return response.data;
    } catch (error) {
      console.error('Error fetching MCP history:', error);
      throw error;
    }
  },
};

export default mcpService;