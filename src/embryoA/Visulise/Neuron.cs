using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;
using Grasshopper;

namespace Embryo.Visulise
{
    class Neuron
    {
        private Point3d pos;
        private double[] vec;
        private Guid instance;
        private Point3d limbo;
        private int[] id;
        private bool isWinner;

        public Neuron(Point3d pos, double[] vec, Guid instance, Point3d limbo, int[] id){
            this.pos = pos;
            this.vec = vec;
            this.instance = instance;
            this.limbo = limbo;
            this.id = id;
            this.isWinner = false;
        }

        public void organise(Neuron[][] neuron, Input[] input, int i, int[] winner, double WINLEARN, double LEARN, double NEIGH)
        {

            if (id[0] == winner[0] && id[1] == winner[1])
            {

                isWinner = true;

                for (int f = 0; f < input[i].vec.Length; f++)
                {
                    double dd = input[i].vec[f] - vec[f];
                    limbo[f] = limbo[f] + dd * WINLEARN;
                }
            }
            // If you were not the winner, check your distance in map 2d space
            // And adjust according to the distance and the LEARN value
            else
            {

                double dist = Math.Sqrt(Math.Pow((id[0] - winner[0]), 2) + Math.Pow((id[1] - winner[1]), 2));

                // If we're close enough apply 'positive' feedback
                if (dist <= NEIGH)
                {
                    for (int f = 0; f < input[i].vec.Length; f++)
                    {
                        double dd = input[i].vec[f] - vec[f];
                        limbo[f] = limbo[f] + dd * (LEARN / dist);
                    }
                }
                // Otherwise apply 'inhibitory' feedback
                else
                {
                    for (int f = 0; f < input[i].vec.Length; f++)
                    {
                        double dd = input[i].vec[f] - vec[f];
                        limbo[f] = limbo[f] - dd * (LEARN / dist) * 0.2;
                    }
                }
            }
        }


        public void update()
        {
        
            //if (guid!=null): rs.DeleteObject(self.guid)

            for (int f = 0; f < vec.Length; f++){
                vec[f] = limbo[f];
            }
        
            //Add point
            guid = document.(self.vec);
            
            // How do I just add an object in Rhino?

            // if(self.isWinner): rs.ObjectColor(self.guid, (255,0,0))
        
            // Reset our winner status to false again
            isWinner = false;
        }
    }
}
