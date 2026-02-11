import { createClient } from '@supabase/supabase-js'

const supabaseUrl = 'https://lrsxcnpelnaabwvaeqas.supabase.co'
const supabaseAnonKey = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imxyc3hjbnBlbG5hYWJ3dmFlcWFzIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzA4MTcxODYsImV4cCI6MjA4NjM5MzE4Nn0.DP1o02UFvPYTbkgGHHNjY-sqi4yeSP3flmSQ_cPAuPY'

export const supabase = createClient(supabaseUrl, supabaseAnonKey)