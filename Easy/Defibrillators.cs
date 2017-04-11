using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {

        string LON = Console.ReadLine();
        string LAT = Console.ReadLine();
        Location userLocation = new Location();
        userLocation.longitude = double.Parse(LON.Replace(',', '.'));
        userLocation.latitude = double.Parse(LAT.Replace(',', '.'));

        double bestDistance = double.MaxValue;
        int bestIndex = -1;

        int N = int.Parse(Console.ReadLine());
        Defibrillator[] defibrillators = new Defibrillator[N];

        for (int i = 0; i < N; i++)
        {
            string DEFIB = Console.ReadLine();
            defibrillators[i] = GetDefibrillator(DEFIB);
            double _tempDistance = GetDistance(userLocation, defibrillators[i].Position);

            if (_tempDistance < bestDistance)
            {
                bestDistance = _tempDistance;
                bestIndex = i;
            }
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(defibrillators[bestIndex].ToString());
    }

    public static Defibrillator GetDefibrillator(string message)
    {
        string[] data = message.Split(';');
        int id = int.Parse(data[0]);
        string name = data[1];
        string address = data[2];
        string contactPhone = data[3];
        Location loc = new Location();
        loc.longitude = double.Parse(data[4].Replace(',', '.'));
        loc.latitude = double.Parse(data[5].Replace(',', '.'));

        return new Defibrillator(id, name, address, contactPhone, loc);
    }

    public static double GetDistance(Location userLocation, Location defibrillatorLocation)
    {
        double x = (defibrillatorLocation.longitude - userLocation.longitude) * Math.Cos((userLocation.latitude + defibrillatorLocation.latitude) / 2);

        double y = (defibrillatorLocation.latitude - userLocation.latitude);

        double distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * 6371;

        return distance;
    }
}

public struct Location
{
    public double longitude;
    public double latitude;
}

public class Defibrillator
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string ContactPhone { get; private set; }
    public Location Position { get; private set; }

    public Defibrillator(int id, string name, string address, string contactPhone, Location position)
    {
        ID = id;
        Name = name;
        Address = address;
        ContactPhone = contactPhone;
        Position = position;
    }

    public override String ToString()
    {
        return Name;
    }
}