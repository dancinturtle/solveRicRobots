using System;
using System.Collections.Generic;

namespace RicRobots
{
    class Answer
    {
     private Queue<string> robotColors;
     public Queue<string> RobotColors
     {
         get => robotColors;
     }
     private int totalSteps;
     public int TotalSteps
     {
         get => totalSteps;
     }
     private Queue<int> robotSteps;
     public Queue<int> RobotSteps
     {
         get => robotSteps;
     }
     private Queue<int[]> robotDestinations;
     public Queue<int[]> RobotDestinations
    {
        get => robotDestinations;
    }
     private Queue<Dictionary<string, Object[]>> trackers;
     public Queue<Dictionary<string, Object[]>> Trackers
     {
         get => trackers;
     }
      public Answer(string robotColor, int robotSteps, int[] robotDestination, Dictionary<string, Object[]> robotTracker)
      {
          this.robotColors = new Queue<string>();
          this.robotColors.Enqueue(robotColor);
          this.totalSteps = robotSteps;
          this.robotSteps = new Queue<int>();
          this.robotSteps.Enqueue(robotSteps);
          this.robotDestinations = new Queue<int[]>();
          this.robotDestinations.Enqueue(robotDestination);
          this.trackers = new Queue<Dictionary<string, Object[]>>();
          this.trackers.Enqueue(robotTracker);
      }
      public void addRobot(string robotColor, int robotSteps, int[] robotDestination, Dictionary<string, Object[]> robotTracker)
      {
          this.robotColors.Enqueue(robotColor);
          this.totalSteps += robotSteps;
          this.robotSteps.Enqueue(robotSteps);
          this.robotDestinations.Enqueue(robotDestination);
          this.trackers.Enqueue(robotTracker);
      }
      public void printAnswer()
      {
          Console.WriteLine($"Answer in {this.totalSteps} steps");
          int[] steps = robotSteps.ToArray();
          string[] robots = this.robotColors.ToArray();
          int[][] destinations = this.robotDestinations.ToArray();
          Dictionary<string, Object[]>[] robotPaths = this.trackers.ToArray();

          for(int i=0; i<robots.Length; i++)
          {
              int[] currentDestination = destinations[i];
              string currDestString = currentDestination[0].ToString() + "-" + currentDestination[1].ToString();
              Console.WriteLine($"Move {robots[i]} {steps[i]} steps to {currDestString}.");
              Dictionary<string, Object[]> currentPath = robotPaths[i];
              string[] traceSteps = new string[steps[i] + 1];
              for(int j=0; j<traceSteps.Length; j++)
              {
                  traceSteps[j] = currDestString;
                  Node stepNode = (Node) currentPath[currDestString][1];
                  if(stepNode != null)
                  {
                    currDestString = stepNode.Name;
                  }
              }
              string traceString = "";
              for(int k=traceSteps.Length - 1; k>=0; k--)
              {
                  traceString += traceSteps[k] + ", ";
              }
              Console.WriteLine(traceString);

          }
      }

    }

}
