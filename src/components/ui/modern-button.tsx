
import * as React from "react";
import { Slot } from "@radix-ui/react-slot";
import { cva, type VariantProps } from "class-variance-authority";
import { cn } from "@/lib/utils";
import { Loader2 } from "lucide-react";

const modernButtonVariants = cva(
  "inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-lg text-sm font-semibold ring-offset-background transition-all duration-300 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 relative overflow-hidden group",
  {
    variants: {
      variant: {
        default: "bg-gradient-to-r from-primary to-primary/90 text-primary-foreground shadow-lg hover:shadow-xl hover:from-primary/90 hover:to-primary transform hover:scale-105 active:scale-95",
        destructive: "bg-gradient-to-r from-destructive to-destructive/90 text-destructive-foreground shadow-lg hover:shadow-xl hover:from-destructive/90 hover:to-destructive transform hover:scale-105 active:scale-95",
        outline: "border-2 border-primary bg-background text-primary hover:bg-primary hover:text-primary-foreground transform hover:scale-105 active:scale-95 shadow-md hover:shadow-lg",
        secondary: "bg-gradient-to-r from-secondary to-secondary/90 text-secondary-foreground shadow-lg hover:shadow-xl hover:from-secondary/90 hover:to-secondary transform hover:scale-105 active:scale-95",
        ghost: "hover:bg-accent hover:text-accent-foreground hover:shadow-md transform hover:scale-105 active:scale-95",
        link: "text-primary underline-offset-4 hover:underline transform hover:scale-105 active:scale-95",
        gradient: "bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500 text-white shadow-lg hover:shadow-xl transform hover:scale-105 active:scale-95 hover:from-blue-600 hover:via-purple-600 hover:to-pink-600",
        glow: "bg-primary text-primary-foreground shadow-lg hover:shadow-primary/25 hover:shadow-2xl transform hover:scale-105 active:scale-95 before:absolute before:inset-0 before:rounded-lg before:bg-gradient-to-r before:from-transparent before:via-white/10 before:to-transparent before:translate-x-[-100%] hover:before:translate-x-[100%] before:transition-transform before:duration-700",
      },
      size: {
        default: "h-12 px-6 py-3",
        sm: "h-10 rounded-md px-4",
        lg: "h-14 rounded-lg px-8 text-base",
        xl: "h-16 rounded-lg px-10 text-lg",
        icon: "h-12 w-12",
      },
    },
    defaultVariants: {
      variant: "default",
      size: "default",
    },
  }
);

export interface ModernButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof modernButtonVariants> {
  asChild?: boolean;
  loading?: boolean;
  icon?: React.ReactNode;
  rightIcon?: React.ReactNode;
}

const ModernButton = React.forwardRef<HTMLButtonElement, ModernButtonProps>(
  ({ className, variant, size, asChild = false, loading, icon, rightIcon, children, disabled, ...props }, ref) => {
    const Comp = asChild ? Slot : "button";
    
    return (
      <Comp
        className={cn(modernButtonVariants({ variant, size, className }))}
        ref={ref}
        disabled={disabled || loading}
        {...props}
      >
        {loading && <Loader2 className="w-4 h-4 animate-spin" />}
        {!loading && icon && <span className="w-4 h-4 flex items-center justify-center">{icon}</span>}
        <span className="relative z-10">{children}</span>
        {!loading && rightIcon && <span className="w-4 h-4 flex items-center justify-center">{rightIcon}</span>}
        
        {/* Shimmer effect for certain variants */}
        {(variant === "default" || variant === "gradient" || variant === "glow") && (
          <div className="absolute inset-0 rounded-lg bg-gradient-to-r from-transparent via-white/20 to-transparent transform -skew-x-12 -translate-x-full group-hover:translate-x-full transition-transform duration-700 pointer-events-none" />
        )}
      </Comp>
    );
  }
);

ModernButton.displayName = "ModernButton";

export { ModernButton, modernButtonVariants };
