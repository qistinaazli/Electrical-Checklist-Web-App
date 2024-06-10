using Npgsql;
using Dapper;
using System.Threading.Tasks;
using FirstWebApp.Components.Pages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace FirstWebApp.Service
{
    public class DbHelper : IDbHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;

        private string _connectionString = "Server=192.168.0.178; Port=7000; Database=checklist_db; User Id=postgres; Password=mplex1234;";

        private string CreateJWT(User user)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("524C1F28-6115-4E16-9B6A-3FBF185308F2")); // NOTE: SAME KEY AS USED IN Program.cs FILE; DO NOT REUSE THIS GUID
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[] // NOTE: could also use List<Claim> here
			{
                new Claim(ClaimTypes.Name, user.email), 
				new Claim(JwtRegisteredClaimNames.Sub, user.email),
                new Claim(JwtRegisteredClaimNames.Email, user.email),
                new Claim(JwtRegisteredClaimNames.Jti, user.email),
                new Claim(ClaimTypes.Role, user.user_type switch{0=>"Admin",1=>"Staff",2=>"SuperAdmin"}),
			};

            var token = new JwtSecurityToken(issuer: "domain.com", audience: "domain.com", claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials); // NOTE: ENTER DOMAIN HERE
            var jsth = new JwtSecurityTokenHandler();
            return jsth.WriteToken(token);
        }

        public DbHelper(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }
        public async Task <List<T>> Fetch<T>(string command)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    var items = (await conn.QueryAsync<T>(command)).ToList();
                    return items;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<T>();
            }

        }

        public async void Save(string command)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    await conn.ExecuteAsync(command);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailOrName(string emailOrName)
        {
            try
            {
                // Execute the SQL query using Dapper
                var user = (await _dbConnection.QueryAsync<User>("SELECT * FROM users WHERE email = @EmailOrName OR name = @EmailOrName", new { EmailOrName = emailOrName })).FirstOrDefault();

                user.jwt_token = CreateJWT(user);

                return user;

                // Return the first user found, or null if none is found
                //return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        public async Task InsertElectricalChecklistAsync(ElectricalChecklistModel model, List<ChecklistItem> ChecklistItemsA, List<ChecklistItem> ChecklistItemsB, 
            List<ChecklistItem> ChecklistItemsC, List<ChecklistItem> ChecklistItemsD, List<ChecklistItem> ChecklistItemsE, List<ChecklistItem> ChecklistItemsF)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var command = new NpgsqlCommand(
                "INSERT INTO checklist_responses(model_num, machine_serial_num, motor_serial_num, pump_serial_num,name, phone_number, email, verified, " +
                "ac_terminal_block, mcb_correct, power_isolator_switch, elcb_spec, stepdown_transformer, dc_power_supply, thermal_motor_spec, " +
                "thermal_correct, emergency_stop_button, motor_pump_wiring, indicator_lamp, float_sensor, push_button, components_correct, " +
                "cable_correct, wiring_correct, wire_sleeves, wire_damage, strain_relief, wiring_following_drawing, wire_color_code, " +
                "no_direct_contact, correct_wiring, indicator_component, emo_function, pump_wiring, no_active_conductor, circuit_correct, " +
                "documentation_complete, connection_from_terminal, isolator_to_elcb, outelcb_to_mcb, connection_mcb_transformer, " +
                "connection_transformer_mcb, mcb_to_dc_power, elcb_to_inmcb, mcb_to_contactor, mc_to_tor, tor_to_motor, grounding_terminal, " +
                "level_sensor, voltage_phase1_neutral, multi_read_phase1, voltage_phase2_neutral, multi_read_phase2, voltage_phase3_neutral, multi_read_phase3) " +
                "VALUES (@model_num, @machine_serial_num,@motor_serial_num,@pump_serial_num, @name, @phone_number, @email, @verified, " +
                "@ac_terminal_block, @mcb_correct, @power_isolator_switch, @elcb_spec, @stepdown_transformer, @dc_power_supply, " +
                "@thermal_motor_spec, @thermal_correct, @emergency_stop_button, @motor_pump_wiring, @indicator_lamp, @float_sensor, " +
                "@push_button, @components_correct, @cable_correct, @wiring_correct, @wire_sleeves, @wire_damage, @strain_relief, " +
                "@wiring_following_drawing, @wire_color_code, @no_direct_contact, @correct_wiring, @indicator_component, @emo_function, " +
                "@pump_wiring, @no_active_conductor, @circuit_correct, @documentation_complete, @connection_from_terminal, " +
                "@isolator_to_elcb, @outelcb_to_mcb, @connection_mcb_transformer, @connection_transformer_mcb, @mcb_to_dc_power, " +
                "@elcb_to_inmcb, @mcb_to_contactor, @mc_to_tor, @tor_to_motor, @grounding_terminal, @level_sensor, @voltage_phase1_neutral, @multi_read_phase1," +
                "@voltage_phase2_neutral,@multi_read_phase2, @voltage_phase3_neutral, @multi_read_phase3)", conn);


            command.Parameters.AddWithValue("model_num", model.model_num);
            command.Parameters.AddWithValue("machine_serial_num", model.machine_serial_num);
            command.Parameters.AddWithValue("motor_serial_num", model.motor_serial_num);
            command.Parameters.AddWithValue("pump_serial_num", model.pump_serial_num);
            command.Parameters.AddWithValue("name", model.name);
            command.Parameters.AddWithValue("phone_number", model.phone_number);
            command.Parameters.AddWithValue("email", model.email);
            command.Parameters.AddWithValue("verified", model.verified);
            
            //Part A
            command.Parameters.AddWithValue("ac_terminal_block", Convert.ToBoolean(ChecklistItemsA.Find(x=>x.Id == "ac_terminal_block").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("mcb_correct", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "mcb_correct").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("power_isolator_switch", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "power_isolator_switch").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("elcb_spec", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "elcb_spec").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("stepdown_transformer", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "stepdown_transformer").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("dc_power_supply", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "dc_power_supply").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("thermal_motor_spec", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "thermal_motor_spec").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("thermal_correct", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "thermal_correct").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("emergency_stop_button", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "emergency_stop_button").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("motor_pump_wiring", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "motor_pump_wiring").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("indicator_lamp", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "indicator_lamp").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("float_sensor", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "float_sensor").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("push_button", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "push_button").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("components_correct", Convert.ToBoolean(ChecklistItemsA.Find(x => x.Id == "components_correct").SelectedOptionAsBoolean));

            //Part B
            command.Parameters.AddWithValue("cable_correct", Convert.ToBoolean(ChecklistItemsB.Find(x => x.Id == "cable_correct").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("wiring_correct", Convert.ToBoolean(ChecklistItemsB.Find(x => x.Id == "wiring_correct").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("wire_sleeves", Convert.ToBoolean(ChecklistItemsB.Find(x => x.Id == "wire_sleeves").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("wire_damage", Convert.ToBoolean(ChecklistItemsB.Find(x => x.Id == "wire_damage").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("strain_relief", Convert.ToBoolean(ChecklistItemsB.Find(x => x.Id == "strain_relief").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("wiring_following_drawing", Convert.ToBoolean(ChecklistItemsB.Find(x => x.Id == "wiring_following_drawing").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("wire_color_code", Convert.ToBoolean(ChecklistItemsB.Find(x => x.Id == "wire_color_code").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("no_direct_contact", Convert.ToBoolean(ChecklistItemsB.Find(x => x.Id == "no_direct_contact").SelectedOptionAsBoolean));

            //Part C
            command.Parameters.AddWithValue("correct_wiring", Convert.ToBoolean(ChecklistItemsC.Find(x => x.Id == "correct_wiring").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("indicator_component", Convert.ToBoolean(ChecklistItemsC.Find(x => x.Id == "indicator_component").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("emo_function", Convert.ToBoolean(ChecklistItemsC.Find(x => x.Id == "emo_function").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("pump_wiring", Convert.ToBoolean(ChecklistItemsC.Find(x => x.Id == "pump_wiring").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("no_active_conductor", Convert.ToBoolean(ChecklistItemsC.Find(x => x.Id == "no_active_conductor").SelectedOptionAsBoolean));

            //Part D
            command.Parameters.AddWithValue("circuit_correct", Convert.ToBoolean(ChecklistItemsD.Find(x => x.Id == "circuit_correct").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("documentation_complete", Convert.ToBoolean(ChecklistItemsD.Find(x => x.Id == "documentation_complete").SelectedOptionAsBoolean));
            
            //Part E
            command.Parameters.AddWithValue("connection_from_terminal", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "connection_from_terminal").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("isolator_to_elcb", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "isolator_to_elcb").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("outelcb_to_mcb", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "outelcb_to_mcb").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("connection_mcb_transformer", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "connection_mcb_transformer").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("connection_transformer_mcb", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "connection_transformer_mcb").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("mcb_to_dc_power", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "mcb_to_dc_power").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("elcb_to_inmcb", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "elcb_to_inmcb").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("mcb_to_contactor", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "mcb_to_contactor").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("mc_to_tor", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "mc_to_tor").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("tor_to_motor", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "tor_to_motor").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("grounding_terminal", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "grounding_terminal").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("level_sensor", Convert.ToBoolean(ChecklistItemsE.Find(x => x.Id == "level_sensor").SelectedOptionAsBoolean));

            // Find and add the parameters
            command.Parameters.AddWithValue("voltage_phase1_neutral", Convert.ToBoolean(ChecklistItemsF.Find(x => x.Id == "voltage_phase1_neutral").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("multi_read_phase1", ChecklistItemsF.Find(x => x.Id == "voltage_phase1_neutral").MultimeterReading);
            command.Parameters.AddWithValue("voltage_phase2_neutral", Convert.ToBoolean(ChecklistItemsF.Find(x => x.Id == "voltage_phase2_neutral").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("multi_read_phase2", ChecklistItemsF.Find(x => x.Id == "voltage_phase2_neutral").MultimeterReading);
            command.Parameters.AddWithValue("voltage_phase3_neutral", Convert.ToBoolean(ChecklistItemsF.Find(x => x.Id == "voltage_phase3_neutral").SelectedOptionAsBoolean));
            command.Parameters.AddWithValue("multi_read_phase3", ChecklistItemsF.Find(x => x.Id == "voltage_phase3_neutral").MultimeterReading);

            await command.ExecuteNonQueryAsync();
        }

        public async Task ExecuteRawSqlAsync(string sql, object parameters)
        {
            await _dbConnection.ExecuteAsync(sql, parameters);
        }
    }
}

