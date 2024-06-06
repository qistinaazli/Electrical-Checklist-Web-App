using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApp
{
    public class DataFetch
    {
        public int? id { get; set; }
        public string? model_num {  get; set; }
        public string? machine_serial_num { get; set; }
        public string? motor_serial_num { get; set; }
        public string? pump_serial_num { get; set; }
        public string? name { get; set; }
        public string? phone_number { get; set; }
        public string? email { get; set; }
        public string? verified { get; set; }

        //Checklist Question
        public string? ac_terminal_block { get; set; }
        public string? mcb_correct { get; set; }
        public string? power_isolator_switch { get; set; }
        public string? elcb_spec { get; set; }
        public string? stepdown_transformer { get; set; }
        public string? dc_power_supply { get; set; }
        public string? thermal_motor_spec { get; set; }
        public string? thermal_correct { get; set; }
        public string? emergency_stop_button { get; set; }
        public string? motor_pump_wiring { get; set; }
        public string? indicator_lamp { get; set; }
        public string? float_sensor { get; set; }
        public string? push_button { get; set; }
        public string? components_correct { get; set; }
        public string? cable_correct { get; set; }
        public string? wiring_correct { get; set; }
        public string? wire_sleeves { get; set; }
        public string? wire_damage { get; set; }
        public string? strain_relief { get; set; }
        public string? wiring_following_drawing { get; set; }
        public string? wire_color_code { get; set; }
        public string? no_direct_contact { get; set; }
        public string? correct_wiring { get; set; }
        public string? indicator_component { get; set; }
        public string? emo_function { get; set; }
        public string? pump_wiring { get; set; }
        public string? no_active_conductor { get; set; }
        public string? circuit_correct { get; set; }
        public string? documentation_complete { get; set; }
        public string? connection_from_terminal { get; set; }
        public string? isolator_to_elcb { get; set; }
        public string? outelcb_to_mcb { get; set; }
        public string? connection_mcb_transformer { get; set; }
        public string? connection_transformer_mcb { get; set; }
        public string? mcb_to_dc_power { get; set; }
        public string? elcb_to_inmcb { get; set; }
        public string? mcb_to_contactor { get; set; }
        public string? mc_to_tor { get; set; }
        public string? tor_to_motor { get; set; }
        public string? grounding_terminal { get; set; }
        public string? level_sensor { get; set; }
        public string? voltage_phase1_neutral { get; set; }
        public string? multi_read_phase1 { get; set; }
        public string? voltage_phase2_neutral { get; set; }
        public string? multi_read_phase2 { get; set; }
        public string? voltage_phase3_neutral { get; set; }
        public string? multi_read_phase3 { get; set; }
    }
}
