
import { Card, CardContent, CardHeader } from "@/components/ui/card";
import { Star, Quote } from "lucide-react";

export default function TestimonialsSection() {
  const testimonials = [
    {
      id: 1,
      name: "Sarah Chen",
      role: "CEO, TechStart",
      company: "TechStart Inc.",
      image: "/placeholder.svg",
      rating: 5,
      text: "AICO transformed our conversion rates completely. We saw a 45% increase in sales within the first month. The AI recommendations were spot-on and easy to implement."
    },
    {
      id: 2,
      name: "Marcus Johnson",
      role: "Marketing Director",
      company: "E-commerce Pro",
      image: "/placeholder.svg",
      rating: 5,
      text: "The real-time analytics dashboard is incredible. We can see exactly what's working and what isn't. AICO pays for itself within weeks."
    },
    {
      id: 3,
      name: "Emily Rodriguez",
      role: "Founder",
      company: "Digital Solutions",
      image: "/placeholder.svg",
      rating: 5,
      text: "As a small business owner, I needed something that just works. AICO's AI does all the heavy lifting, and I focus on running my business. Highly recommended!"
    }
  ];

  return (
    <section className="container mx-auto px-4 py-20 bg-gradient-to-r from-blue-50/50 to-purple-50/50 dark:from-gray-800/50 dark:to-gray-700/50">
      <div className="text-center mb-16">
        <h2 className="text-4xl font-bold mb-4">What Our Customers Say</h2>
        <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
          Join thousands of satisfied customers who have transformed their businesses with AICO.
        </p>
      </div>
      
      <div className="grid md:grid-cols-3 gap-8">
        {testimonials.map((testimonial) => (
          <Card key={testimonial.id} className="relative">
            <CardHeader>
              <div className="absolute top-4 right-4 text-primary">
                <Quote className="w-8 h-8 opacity-20" />
              </div>
              <div className="flex items-center space-x-4">
                <div className="w-12 h-12 bg-gradient-to-r from-blue-500 to-purple-600 rounded-full flex items-center justify-center text-white font-bold">
                  {testimonial.name.split(' ').map(n => n[0]).join('')}
                </div>
                <div>
                  <h4 className="font-semibold">{testimonial.name}</h4>
                  <p className="text-sm text-muted-foreground">{testimonial.role}</p>
                  <p className="text-sm text-muted-foreground">{testimonial.company}</p>
                </div>
              </div>
              <div className="flex items-center space-x-1 mt-2">
                {[...Array(testimonial.rating)].map((_, i) => (
                  <Star key={i} className="w-4 h-4 fill-yellow-400 text-yellow-400" />
                ))}
              </div>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground italic">"{testimonial.text}"</p>
            </CardContent>
          </Card>
        ))}
      </div>
    </section>
  );
}
