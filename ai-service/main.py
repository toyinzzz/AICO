# FastAPI entry point 
import fastapi 
from fastapi import FastAPI 
 
app = FastAPI( 
    title="AICO AI Analysis Service", 
    description="AI-powered website analysis and recommendations", 
    version="0.1.0" 
) 
 
@app.get("/") 
def read_root(): 
    return {"message": "Welcome to AICO AI Analysis Service"} 
