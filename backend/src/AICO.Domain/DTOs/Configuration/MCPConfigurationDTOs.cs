using System;
using System.Collections.Generic;

namespace AICO.Domain.DTOs.Configuration;

/// <summary>
/// Report scheduling configuration
/// </summary>
public record MCPReportSchedule(
    string ScheduleId,
    string ReportType,
    string ScheduleName,
    MCPScheduleFrequency Frequency,
    MCPScheduleSettings Settings,
    List<string> Recipients,
    MCPDeliveryOptions DeliveryOptions,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastRun,
    DateTime? NextRun
);

/// <summary>
/// Schedule frequency configuration
/// </summary>
public record MCPScheduleFrequency(
    string FrequencyType,
    int Interval,
    List<string> DaysOfWeek,
    int DayOfMonth,
    TimeSpan TimeOfDay,
    string TimeZone
);

/// <summary>
/// Schedule-specific settings
/// </summary>
public record MCPScheduleSettings(
    Dictionary<string, object> Parameters,
    List<string> Filters,
    string DateRange,
    bool IncludeCharts,
    string OutputFormat
);

/// <summary>
/// Delivery options for scheduled reports
/// </summary>
public record MCPDeliveryOptions(
    List<string> DeliveryMethods,
    MCPEmailSettings EmailSettings,
    MCPWebhookSettings WebhookSettings,
    MCPFileStorageSettings FileStorageSettings
);

/// <summary>
/// Email delivery configuration
/// </summary>
public record MCPEmailSettings(
    string Subject,
    string Body,
    bool IncludeAttachment,
    string AttachmentFormat,
    List<string> CcRecipients,
    List<string> BccRecipients
);

/// <summary>
/// Webhook delivery configuration
/// </summary>
public record MCPWebhookSettings(
    string WebhookUrl,
    Dictionary<string, string> Headers,
    string AuthenticationType,
    Dictionary<string, object> AuthenticationConfig,
    int RetryAttempts,
    int TimeoutSeconds
);

/// <summary>
/// File storage delivery configuration
/// </summary>
public record MCPFileStorageSettings(
    string StorageType,
    string StoragePath,
    string FileNamePattern,
    Dictionary<string, object> StorageConfig,
    bool CompressFiles,
    int RetentionDays
);

/// <summary>
/// Report configuration and customization
/// </summary>
public record MCPReportConfiguration(
    string ConfigurationId,
    string ReportType,
    string ConfigurationName,
    MCPReportLayout Layout,
    MCPReportFilters Filters,
    MCPReportMetrics Metrics,
    MCPReportFormatting Formatting,
    MCPReportPermissions Permissions,
    DateTime CreatedAt,
    string CreatedBy
);

/// <summary>
/// Report layout configuration
/// </summary>
public record MCPReportLayout(
    List<string> Sections,
    Dictionary<string, MCPSectionConfig> SectionConfigs,
    string PageOrientation,
    string PageSize,
    MCPHeaderFooterConfig HeaderFooter
);

/// <summary>
/// Configuration for individual report sections
/// </summary>
public record MCPSectionConfig(
    string SectionType,
    int Order,
    bool IsVisible,
    Dictionary<string, object> SectionSettings,
    MCPChartConfig ChartConfig
);

/// <summary>
/// Chart configuration for report sections
/// </summary>
public record MCPChartConfig(
    string ChartType,
    Dictionary<string, object> ChartSettings,
    MCPChartStyling Styling,
    bool ShowLegend,
    bool ShowDataLabels
);

/// <summary>
/// Chart styling configuration
/// </summary>
public record MCPChartStyling(
    Dictionary<string, string> Colors,
    string FontFamily,
    int FontSize,
    Dictionary<string, object> CustomStyles
);

/// <summary>
/// Header and footer configuration
/// </summary>
public record MCPHeaderFooterConfig(
    string HeaderText,
    string FooterText,
    bool IncludeLogo,
    bool IncludePageNumbers,
    bool IncludeTimestamp
);

/// <summary>
/// Report filtering configuration
/// </summary>
public record MCPReportFilters(
    List<MCPFilterRule> FilterRules,
    string DateRangeType,
    DateTime? StartDate,
    DateTime? EndDate,
    List<string> IncludedVariants,
    List<string> ExcludedSegments
);

/// <summary>
/// Individual filter rule
/// </summary>
public record MCPFilterRule(
    string FieldName,
    string Operator,
    object Value,
    string LogicalOperator
);

/// <summary>
/// Report metrics configuration
/// </summary>
public record MCPReportMetrics(
    List<string> IncludedMetrics,
    List<string> ExcludedMetrics,
    Dictionary<string, MCPMetricConfig> MetricConfigs,
    List<MCPCalculatedMetric> CalculatedMetrics
);

/// <summary>
/// Configuration for individual metrics
/// </summary>
public record MCPMetricConfig(
    string DisplayName,
    string Format,
    int DecimalPlaces,
    bool ShowTrend,
    string AggregationType
);

/// <summary>
/// Calculated metric definition
/// </summary>
public record MCPCalculatedMetric(
    string MetricName,
    string Formula,
    string Description,
    List<string> Dependencies,
    string DataType
);

/// <summary>
/// Report formatting configuration
/// </summary>
public record MCPReportFormatting(
    MCPColorScheme ColorScheme,
    MCPFontSettings FontSettings,
    MCPTableFormatting TableFormatting,
    Dictionary<string, object> CustomFormatting
);

/// <summary>
/// Color scheme for reports
/// </summary>
public record MCPColorScheme(
    string SchemeName,
    string PrimaryColor,
    string SecondaryColor,
    string AccentColor,
    List<string> ChartColors
);

/// <summary>
/// Font settings for reports
/// </summary>
public record MCPFontSettings(
    string FontFamily,
    int HeaderFontSize,
    int BodyFontSize,
    int FooterFontSize,
    bool UseBoldHeaders
);

/// <summary>
/// Table formatting configuration
/// </summary>
public record MCPTableFormatting(
    bool AlternateRowColors,
    bool ShowBorders,
    string BorderStyle,
    string HeaderBackgroundColor,
    string HeaderTextColor
);

/// <summary>
/// Report permissions and access control
/// </summary>
public record MCPReportPermissions(
    List<string> ViewerRoles,
    List<string> EditorRoles,
    List<string> AdminRoles,
    bool IsPublic,
    MCPDataSensitivity DataSensitivity,
    List<MCPAccessRestriction> AccessRestrictions
);

/// <summary>
/// Data sensitivity classification
/// </summary>
public record MCPDataSensitivity(
    string SensitivityLevel,
    List<string> SensitiveFields,
    bool RequiresApproval,
    string ApprovalWorkflow
);

/// <summary>
/// Access restrictions for reports
/// </summary>
public record MCPAccessRestriction(
    string RestrictionType,
    List<string> AllowedUsers,
    List<string> AllowedGroups,
    Dictionary<string, object> RestrictionConfig
);

/// <summary>
/// Report template definition
/// </summary>
public record MCPReportTemplate(
    string TemplateId,
    string TemplateName,
    string TemplateType,
    string Description,
    MCPTemplateStructure Structure,
    MCPTemplateSettings Settings,
    List<MCPTemplateVariable> Variables,
    bool IsSystemTemplate,
    DateTime CreatedAt,
    string CreatedBy
);

/// <summary>
/// Template structure definition
/// </summary>
public record MCPTemplateStructure(
    List<MCPTemplateSection> Sections,
    MCPTemplateLayout Layout,
    Dictionary<string, object> DefaultValues
);

/// <summary>
/// Individual template section
/// </summary>
public record MCPTemplateSection(
    string SectionId,
    string SectionName,
    string SectionType,
    int Order,
    Dictionary<string, object> SectionConfig,
    List<MCPTemplateField> Fields
);

/// <summary>
/// Template field definition
/// </summary>
public record MCPTemplateField(
    string FieldId,
    string FieldName,
    string FieldType,
    bool IsRequired,
    object DefaultValue,
    Dictionary<string, object> FieldConfig
);

/// <summary>
/// Template layout configuration
/// </summary>
public record MCPTemplateLayout(
    string LayoutType,
    Dictionary<string, object> LayoutConfig,
    MCPResponsiveSettings ResponsiveSettings
);

/// <summary>
/// Responsive design settings for templates
/// </summary>
public record MCPResponsiveSettings(
    Dictionary<string, MCPBreakpointConfig> Breakpoints,
    bool IsResponsive,
    string DefaultBreakpoint
);

/// <summary>
/// Breakpoint configuration for responsive design
/// </summary>
public record MCPBreakpointConfig(
    int MinWidth,
    int MaxWidth,
    Dictionary<string, object> BreakpointSettings
);

/// <summary>
/// Template settings and configuration
/// </summary>
public record MCPTemplateSettings(
    bool AllowCustomization,
    List<string> EditableFields,
    MCPValidationRules ValidationRules,
    Dictionary<string, object> AdvancedSettings
);

/// <summary>
/// Validation rules for templates
/// </summary>
public record MCPValidationRules(
    List<MCPValidationRule> Rules,
    bool StrictValidation,
    string ValidationMode
);

/// <summary>
/// Individual validation rule
/// </summary>
public record MCPValidationRule(
    string RuleId,
    string FieldName,
    string ValidationType,
    Dictionary<string, object> ValidationConfig,
    string ErrorMessage
);

/// <summary>
/// Template variable definition
/// </summary>
public record MCPTemplateVariable(
    string VariableName,
    string VariableType,
    object DefaultValue,
    string Description,
    bool IsRequired,
    Dictionary<string, object> VariableConfig
);