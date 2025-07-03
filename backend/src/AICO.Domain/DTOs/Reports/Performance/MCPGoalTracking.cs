using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Goal tracking and achievement metrics
/// </summary>
public record MCPGoalTracking(
    List<MCPGoalMetric> Goals,
    decimal OverallGoalAchievement,
    List<string> AchievedGoals,
    List<string> MissedGoals
);

/// <summary>
/// Individual goal metric tracking
/// </summary>
public record MCPGoalMetric(
    string GoalName,
    string GoalType,
    decimal TargetValue,
    decimal ActualValue,
    decimal AchievementPercentage,
    string Status
);