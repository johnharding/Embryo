using System;
using System.Drawing;
using Grasshopper.Kernel;
using Embryo.Properties;
using System.Collections.Generic;

// TODO: Custom attributes

namespace Embryo.Visulise
{
    public class Kohonen : GH_Component
    {
        int xMap =30;
        int yMap =30;
        int SPACE = 12;
        int POP = 8;
        double strength = 0.2;
        double BOT = 1; //minimum radius
        int FEATURES = 3;
        double WINLEARN, LEARN, NEIGH, RADIO, cycles;
        bool CONVERGED;
        int[] winner = new int[2];
                                        //features & limbo & pos
        float[][][] som;
        float[][] inputs;

        public Kohonen()
            : base("Kohonen Self-Organising Map", "Kohonen", "Dimensionality Reduction using an Artificial Neural Network", "Embryo", "Utilities")
        {
            som = new float[xMap][yMap][FEATURES*2+2];
            inputs = new float[POP][FEATURES+2];

            cycles = 0;
              WINLEARN = 0.95;
              LEARN = 0.85;
              RADIO = sqrt(pow(xMap,2)+pow(yMap,2))/2;
              //make the map
              for (int i=0; i<xMap; i++){
                for (int j=0; j<yMap; j++){
                  som[i][j][0] = i * SPACE;
                  som[i][j][1] = j * SPACE;
                    for (int u=2; u<(FEATURES+2); u++){
                      som[i][j][u] = som[i][j][u+FEATURES] = Random(255); // let the limbo be the same
                    }
                }
              }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.Register_DoubleParam("Data      ", "Data      ", "data... data... data... yum");
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        //SolveInstance is a method in the GH_Component class
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double myFloat = 0.0;
            string myString = "";
            //double myFloat2 = 0.0;
            if (!DA.GetData(0, ref myFloat)) { return; }
            if (!DA.GetData(1, ref myString)) { return; }
            //if (!DA.GetData(2, ref myFloat2)) { return; }
            
            //initialise the inputs. note that the positions start at 0,0
            for (int m=0; m<POP; m++){
                for (int n=2; n<FEATURES+2; n++){
                     inputs[m][n] = int(random(255));
                }
            }
            

  


void draw(){
  
  cycles++;

  //draw the SOM
  for (int i=0; i<xMap; i++){
    for (int j=0; j<yMap; j++){
      
      // 2R 3G 4B
      fill(som[i][j][2], som[i][j][3], som[i][j][4]);
      rect(som[i][j][0],som[i][j][1],SPACE,SPACE);

    }
  }

  //draw the inputs
  for (int m=0; m<POP; m++){
    stroke(255);
    fill(inputs[m][2],inputs[m][3],inputs[m][4]);
    ellipse(inputs[m][0],inputs[m][1],SPACE,SPACE);
    stroke(0);
  }

  noStroke();
  for (int i=0; i<POP; i++){
    find_winner(i);
    organise_map(i);
  }
  
  update_map();
 
}

void find_winner(int muk){
  float mindis=10000;  //local variable mindis
  
    for (int i=0; i<xMap; i++){
      for (int j=0; j<yMap; j++){
        float dis = 0;
          for (int u=2; u<FEATURES+2; u++){
            dis += pow((som[i][j][u]-inputs[muk][u]),2); // square all the distances
          }
          dis = sqrt(dis); //now route the sum
          if (dis < mindis){mindis=dis; winner[0]=i; winner[1]=j;} //get the reference of the winner
      }
    }
    inputs[muk][0] = winner[0]*SPACE;
    inputs[muk][1] = winner[1]*SPACE;

}

void organise_map(int muk){
  float[] dd = new float[FEATURES];
  float rad;
    for (int i=0; i<xMap; i++){
      for (int j=0; j<yMap; j++){
        if(i!=winner[0] || j!=winner[1]){
          rad = sqrt(pow((i - winner[0]),2) + pow((j - winner[1]),2));
          
          if(rad<=NEIGH){
            for (int u=2; u<FEATURES+2; u++){
            dd[u-2] = inputs[muk][u] - som[i][j][u];
            som[i][j][FEATURES+u] += dd[u-2]*(LEARN/rad);
            }
          }
        }
        else{
          for (int u=2; u<FEATURES+2; u++){
            dd[u-2] = inputs[muk][u] - som[winner[0]][winner[1]][u];
            som[winner[0]][winner[1]][FEATURES+u] += dd[u-2]*WINLEARN;
          }
        }
      }
    }
    //noLoop();
}

void update_map(){
  
  WINLEARN *= (1 - cycles / 300);
  LEARN *= (1 - cycles / 200);
  NEIGH = RADIO*(1 - cycles / 150);

  if(WINLEARN < 0.005){CONVERGED=true;}
    
     for (int i=0; i<xMap; i++){
      for (int j=0; j<yMap; j++){
        for (int u=2; u<FEATURES+2; u++){
          som[i][j][u] = som[i][j][u+FEATURES];
          
        }
      }
     }
}







    



        }

        public override Guid ComponentGuid
        {
            //generated at http://www.newguid.com/
            get { return new Guid("f3b3eccb-b450-4772-92d4-e6c9364c848b "); }
        }

        public override void CreateAttributes()
        {
            m_attributes = new SpiderGraphAttrib(this);
        }

        protected override Bitmap Icon
        {
            get
            {
                return Properties.Resources.Spider_Chart_01;
            }
        }
    }
}

