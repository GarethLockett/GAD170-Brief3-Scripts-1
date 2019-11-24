using UnityEngine;
using UnityEngine.UI;

/*
    Script: UiCharacterHealthBar
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple script fo displaying a character health bar.
                    Add to game object with inherited Character component (eg CharacterPlayer, CharacterEnemy )
                    Will set the X width of the health bar image to a scaled proportion of original width based on current health vs original health.
*/

[ RequireComponent( typeof( Character ) ) ]
public class UiCharacterHealthBar : MonoBehaviour
{
    // Properties
    public Image healthBar;                     // Reference to a UI image to use as health bar.
    public Color maxHealthColor = Color.green;  // 100% health bar color.
    public Color minHealthColor = Color.red;    // 0% health bar color.

    private Character character;                // Reference to the Character component of this game object.
    private RectTransform healthBarRT;          // Reference to the UI image RectTransform (eg for changing length)
    private float originalHealthBarWidth;       // Original health bar width recorded at Start()
    private float originalHealth;               // Original health level of the character recorded at Start()

    // Methods
    private void Start()
    {
        // Get the Character component of this game object.
        this.character = this.gameObject.GetComponent<Character>();

        // Check a health bar has been assigned in the editor.
        if( this.healthBar == null ){ Debug.LogWarning( "No healthBar assigned!?", this.gameObject ); this.enabled = false; return; }

        // Get the RectTransform.
        this.healthBarRT = this.healthBar.gameObject.GetComponent<RectTransform>();
        if( this.healthBarRT == null ){ Debug.LogWarning( "Could NOT get the health bar RectTransform!?", this.gameObject ); this.enabled = false; return; }

        // Record the original health bar width.
        this.originalHealthBarWidth = this.healthBarRT.rect.width;

        // Record the original character health.
        this.originalHealth = this.character.health;
    }

    private void Update()
    {
        // Sanity checks.
        if( this.character == null || this.healthBar == null ){ return; }

        // Calculate the health bar width using the original width, multiplied by the original health divided by the current health.
        float healthScaleAmount = this.originalHealth / this.character.health;
        float newWidth = this.originalHealthBarWidth *healthScaleAmount;

        // Animate the health bar width to the newly calculated width.
        this.healthBarRT.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, newWidth );

        // Set the health bar color.
        this.healthBar.color = Color.Lerp( this.minHealthColor, this.maxHealthColor, healthScaleAmount );
    }
}
