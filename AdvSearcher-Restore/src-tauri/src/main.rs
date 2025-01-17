use tauri::Emitter;
use tauri_dotnet_bridge_host;
use tokio::task;

#[tauri::command]
async fn dotnet_request(request: String) -> String {
    let response = task::spawn_blocking(move || {
        tauri_dotnet_bridge_host::process_request(&request)
    })
    .await
    .expect("Task panicked");
    response
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
fn main() {
    tauri::Builder::default()
            .plugin(tauri_plugin_shell::init())
            .invoke_handler(tauri::generate_handler![dotnet_request])
            .setup(|app| {
                let app_handle = app.handle().clone();
                tauri_dotnet_bridge_host::register_emit(move |event_name, payload| {
                    app_handle
                        .emit(event_name, payload)
                        .expect(&format!("Failed to emit event {}", event_name));
                });
                Ok(())
            })
            .run(tauri::generate_context!())
            .expect("error while running tauri application");
}