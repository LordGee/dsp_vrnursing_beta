using UnityEngine;

/// <summary>
/// Handles the connectors on either end of the pipe piece
/// </summary>
public class PipeConnector : MonoBehaviour
{
    public int connectorIndex;
    private bool connected { get; set; }

    /// <summary>
    /// If connection is lost the pipe piece is reset
    /// </summary>
    private void OnTriggerExit(Collider _col) {
        gameObject.GetComponentInParent<Pipe>().Disconnected();
        connected = false;
    }

    /// <summary>
    /// Once two connectors meet a test is made to see if they are a positive connection
    /// The appropriate outcome is executed depending on the connection result.
    /// </summary>
    private void OnTriggerStay(Collider _col) {
        if (_col.tag == ConstantController.TAG_PIPE_CONNECTOR) {
            if (_col.GetComponentInParent<Pipe>().CheckConnectedSupply()) {
                gameObject.GetComponentInParent<Pipe>().Connected();
                connected = true;
            } else if (gameObject.GetComponentInParent<Pipe>().CheckConnectedSupply()) {
                // Keep object as is
            } else { 
                gameObject.GetComponentInParent<Pipe>().Disconnected();
                connected = false;
            }
        }
    }
}