using System.ComponentModel.DataAnnotations;
using static FirstWebApp.Components.Pages.Home;

namespace FirstWebApp.Components.Pages
{
    public class ElectricalChecklistModel
    {
        [Required(ErrorMessage = "Model number is required.")]
        public string model_num { get; set; }

        [Required(ErrorMessage = "Machine Serial number is required.")]
        public string machine_serial_num { get; set; }

        [Required(ErrorMessage = "Motor Serial number is required.")]
        public string motor_serial_num { get; set; }

        [Required(ErrorMessage = "Pump Serial number is required.")]
        public string pump_serial_num { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string phone_number { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Verified by is required.")]
        public string verified { get; set; }

        
        //Checklist Question
        public bool ac_terminal_block { get; set; }
        public bool mcb_correct { get; set; }
        public bool power_isolator_switch { get; set; }
        public bool elcb_spec { get; set; }
        public bool stepdown_transformer { get; set; }
        public bool dc_power_supply { get; set; }
        public bool thermal_motor_spec { get; set; }
        public bool thermal_correct { get; set; }
        public bool emergency_stop_button { get; set; }
        public bool motor_pump_wiring { get; set; }
        public bool indicator_lamp { get; set; }
        public bool float_sensor { get; set; }
        public bool push_button { get; set; }
        public bool components_correct { get; set; }
        public bool cable_correct { get; set; }
        public bool wiring_correct { get; set; }
        public bool wire_sleeves { get; set; }
        public bool wire_damage { get; set; }
        public bool strain_relief { get; set; }
        public bool wiring_following_drawing { get; set; }
        public bool wire_color_code { get; set; }
        public bool no_direct_contact { get; set; }
        public bool correct_wiring { get; set; }
        public bool indicator_component { get; set; }
        public bool emo_function { get; set; }
        public bool pump_wiring { get; set; }
        public bool no_active_conductor { get; set; }
        public bool circuit_correct { get; set; }
        public bool documentation_complete { get; set; }
        public bool connection_from_terminal { get; set; }
        public bool isolator_to_elcb { get; set; }
        public bool outelcb_to_mcb { get; set; }
        public bool connection_mcb_transformer { get; set; }
        public bool connection_transformer_mcb { get; set; }
        public bool mcb_to_dc_power { get; set; }
        public bool elcb_to_inmcb { get; set; }
        public bool mcb_to_contactor { get; set; }
        public bool mc_to_tor { get; set; }
        public bool tor_to_motor { get; set; }
        public bool grounding_terminal { get; set; }
        public bool level_sensor { get; set; }
        public bool voltage_phase1_neutral { get; set; }
        public bool multi_read_phase1 { get; set; }
        public bool voltage_phase2_neutral { get; set; }
        public bool multi_read_phase2 { get; set; }
        public bool voltage_phase3_neutral { get; set; }
        public bool multi_read_phase3 { get; set; }

    }

    public class ChecklistItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string SelectedOption { get; set; }
        private string _multimeterReading;
        private const string Unit = "V"; // Define the unit

        // Property for database storage
        public string MultimeterReading
        {
            get => _multimeterReading;
            set
            {
                _multimeterReading = value; // Store value without unit
            }
        }

        // Property to store the unit
        public string MultimeterUnit { get; } = Unit;

        public string MultimeterReadingWithUnit => _multimeterReading + " " + Unit;

        public string MultimeterReadingId { get; set; }

        public bool SelectedOptionAsBoolean => SelectedOption == "Yes" ? true : false;
    }

    //Cara lagi simple kalau pakai C#
    //public bool SelectedOptionAsBoolean => SelectedOption == "Yes" ? true : false;

        //public bool SelectedOptionAsBoolean
        //{
        //    get
        //    {
        //        if (SelectedOption == "Yes") return true;
        //        if (SelectedOption == "No") return false;
        //        return null;
        //    }
        //}

}
