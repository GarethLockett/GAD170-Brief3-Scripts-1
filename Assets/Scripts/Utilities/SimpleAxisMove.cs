using UnityEngine;

/*
	Name: SimpleAxisMove
	Author: Gareth Lockett
	Version: 1.0

	Description: This script will move the GameObject it is attached to along the selected axis by a certain speed.
*/

public class SimpleAxisMove : MonoBehaviour
{
	// Constants
	public enum Axis{ xAxis, yAxis, zAxis }

	// Properties
	public float moveSpeed = 1f;	// Speed at which this GameObject will move (eg higher values will move faster)
	public Axis axis;				// The axis to move along.

	// Methods
    void Update()
    {
        // Move this GameObject along the selected axis
        switch( this.axis )
        {
        	case Axis.xAxis:
        		this.transform.position += this.transform.right *Time.deltaTime *this.moveSpeed;
        		break;

			case Axis.yAxis:
        		this.transform.position += this.transform.up *Time.deltaTime *this.moveSpeed;
        		break;

			case Axis.zAxis:
        		this.transform.position += this.transform.forward *Time.deltaTime *this.moveSpeed;
        		break;
        }
    }
}
